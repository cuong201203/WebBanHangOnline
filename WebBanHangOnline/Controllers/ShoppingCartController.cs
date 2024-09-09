using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using ClientApp.Attributes;
using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebBanHangOnline.Controllers
{
    [CustomAuthorize("/account/login")]
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Phương thức để lấy giỏ hàng hiện tại
        private ShoppingCart GetCurrentCart()
        {
            var userId = User.Identity.GetUserId();
            return ShoppingCart.LoadCart(userId, db);
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = GetCurrentCart();
            ViewBag.CheckCart = cart.items.Any(); // Kiểm tra xem giỏ hàng có bất kỳ sản phẩm nào không
            return View(cart.items); // Truyền danh sách sản phẩm cho View
        }


        public ActionResult CheckOut()
        {
            var cart = GetCurrentCart();
            ViewBag.CheckCart = cart;
            return View();
        }

        public ActionResult CheckOutSuccess()
        {
            return View();
        }

        public ActionResult Partial_Item_ThanhToan()
        {
            var cart = GetCurrentCart();
            return PartialView(cart.items);
        }

        public ActionResult Partial_Item_Cart()
        {
            var cart = GetCurrentCart();
            return PartialView(cart.items);
        }

        public ActionResult ShowCount()
        {
            var cart = GetCurrentCart();
            return Json(new { count = cart.items.Count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { success = false, code = -1 };
            if (ModelState.IsValid)
            {
                var cart = GetCurrentCart();
                if (cart != null)
                {
                    Order order = new Order
                    {
                        CustomerName = req.CustomerName,
                        Phone = req.Phone,
                        Address = req.Address,
                        Email = req.Email,
                        TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity)),
                        TypePayment = req.TypePayment,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = req.Phone,
                        CustomerId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null,
                        Code = "DH" + new Random().Next(1000, 9999)
                    };
                    cart.items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        ProductImg = x.ProductImg,
                        Alias = x.Alias,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        TotalPrice = x.TotalPrice,
                        CategoryName = x.CategoryName
                    }));
                    db.Orders.Add(order);
                    db.SaveChanges();
                    // Send email when order is successful
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;
                    foreach (var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + string.Format("{0:N0}", sp.TotalPrice) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    TongTien = thanhtien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", string.Format("{0:N0}", thanhtien));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", string.Format("{0:N0}", TongTien));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                    cart.ClearCart();
                    cart.SaveCart(db);

                    //code = new { success = true, code = 1 };
                    return RedirectToAction("CheckOutSuccess");
                    //return Json(new { success = true, code = 1 });
                    //return Json(new { success = true, code = 1, url = Url.Action("CheckOutSuccess", "ShoppingCart") });
                }
            }
            return Json(code);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { success = false, msg = "", code = -1, count = 0 };
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                var cart = GetCurrentCart();
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault)?.Image,
                    Alias = checkProduct.Alias,
                    Price = (int)((checkProduct.PriceSale > 0) ? checkProduct.PriceSale : checkProduct.Price),
                    Quantity = quantity,
                    TotalPrice = (int)((checkProduct.PriceSale > 0) ? checkProduct.PriceSale : checkProduct.Price) * quantity,
                    CategoryName = checkProduct.ProductCategory.Title
                };
                cart.AddToCart(item, quantity);
                cart.SaveCart(db);
                code = new { success = true, msg = "Thêm sản phẩm thành công", code = 1, count = cart.items.Count };
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var cart = GetCurrentCart();
            cart.Remove(id);
            cart.SaveCart(db);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteAll()
        {
            var cart = GetCurrentCart();
            cart.ClearCart(); // Gọi phương thức xóa toàn bộ giỏ hàng
            cart.SaveCart(db); // Lưu lại giỏ hàng sau khi xóa
            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            var cart = GetCurrentCart();
            cart.UpdateQuantity(id, quantity);
            cart.SaveCart(db);
            return Json(new { success = true });
        }

        public ActionResult Cart()
        {
            var cart = GetCurrentCart();
            ViewBag.CheckCart = cart;
            return View(cart);
        }
    }
}
