using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using ClientApp.Attributes;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;

namespace WebBanHangOnline.Controllers
{
    [CustomAuthorize("/account/login")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

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

        public ActionResult Partial_ItemCheckOut()
        {
            var cart = GetCurrentCart();
            return PartialView(cart.items);
        }

        public ActionResult Partial_ItemCart()
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
        public ActionResult CheckOut(OrderViewModel request)
        {
            if (ModelState.IsValid)
            {
                var cart = GetCurrentCart();
                if (cart != null)
                {
                    Order order = new Order
                    {
                        CustomerName = request.CustomerName,
                        Phone = request.Phone,
                        Address = request.Address,
                        Email = request.Email,
                        TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity)),
                        TypePayment = request.TypePayment,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = request.Phone,
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
                    var thanhTien = decimal.Zero;
                    var tongTien = decimal.Zero;
                    foreach (var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + string.Format("{0:N0}", sp.TotalPrice) + "</td>";
                        strSanPham += "</tr>";
                        thanhTien += sp.Price * sp.Quantity;
                    }
                    tongTien = thanhTien;
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", request.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", string.Format("{0:N0}", thanhTien));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", string.Format("{0:N0}", tongTien));
                    WebBanHangOnline.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), request.Email);

                    cart.ClearCart();
                    cart.SaveCart(db);

                    if (request.TypePayment == 1)
                    {
                        return RedirectToAction("CheckOutSuccess");                     
                    } 
                    else
                    {
                        return Redirect(UrlPayment(request.VnPayTypePayment, order.Code));
                    }                    
                }
            }
            return View();
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

        public ActionResult VnPayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
                var vnPayData = Request.QueryString;
                VnPayLibrary vnPay = new VnPayLibrary();

                foreach (string s in vnPayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnPay.AddResponseData(s, vnPayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnPay.GetResponseData("vnp_TxnRef"));
                long vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnPay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnPay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnPay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            // itemOrder.Status = 2; // Checked out
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        // Successful transaction
                        ViewBag.ThanhToanThanhCong = "số tiền thanh toán (VND): " + vnp_Amount.ToString();
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";                        
                    }
                    else
                    {
                        // Failed transaction. Error code: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý. Mã lỗi: " + vnp_ResponseCode;
                    }                    
                }
            }
            return View();
        }

        #region Thanh toán VNPay
        public string UrlPayment(int vnPayTypePayment, string orderCode)
        {
            var urlPayment = "";
            var order = db.Orders.FirstOrDefault(x => x.Code == orderCode);
            // Get Config Info
            string vnp_ReturnUrl = ConfigurationManager.AppSettings["vnp_ReturnUrl"]; // URL returning transaction response
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; // VNPay check-out URL
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; // Terminal Code
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; // Secret Key

            // Build URL for VNPAY
            VnPayLibrary vnPay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            // Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán
            // là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnPay.AddRequestData("vnp_Amount", Price.ToString()); 
            if (vnPayTypePayment == 1)
            {
                vnPay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (vnPayTypePayment == 2)
            {
                vnPay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (vnPayTypePayment == 3)
            {
                vnPay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnPay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnPay.AddRequestData("vnp_OrderType", "other"); // Default value: other
            vnPay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            vnPay.AddRequestData("vnp_TxnRef", order.Code);

            urlPayment = vnPay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }
        #endregion
    }
}
