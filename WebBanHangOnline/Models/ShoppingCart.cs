using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> items { get; set; }
        public string UserId { get; set; } // Thêm UserId để xác định giỏ hàng của người dùng

        public ShoppingCart(string userId = null)
        {
            this.items = new List<ShoppingCartItem>();
            this.UserId = userId;
        }

        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExist = items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExist != null)
            {
                checkExist.Quantity += Quantity;
                checkExist.TotalPrice = checkExist.Price * checkExist.Quantity;
            }
            else
            {
                items.Add(item);
            }
        }

        public void Remove(int id)
        {
            var checkExist = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                items.Remove(checkExist);
            }
        }

        public void UpdateItemCartQuantity(int id, int quantity)
        {
            var checkExist = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExist != null)
            {
                checkExist.Quantity = quantity;
                checkExist.TotalPrice = checkExist.Price * checkExist.Quantity;
            }
        }

        public void UpdateProductQuantity(Order order, ApplicationDbContext db)
        {
            foreach (var detail in order.OrderDetails)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == detail.ProductId);

                if (product != null)
                {
                    product.Quantity = Math.Max(product.Quantity - detail.Quantity, 0);
                    db.Entry(product).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
        }

        public int GetTotalPrice()
        {
            return items.Sum(x => x.TotalPrice);
        }

        public int GetTotalQuantity()
        {
            return items.Sum(x => x.Quantity);
        }

        public void ClearItemCart(List<int> selectedProductIds) 
        {
            items.RemoveAll(x => selectedProductIds.Contains((int)x.GetType().GetProperty("ProductId").GetValue(x, null)));

        }

        public void ClearAllCart()
        {
            items.Clear();
        }

        // Hàm lưu giỏ hàng vào cơ sở dữ liệu hoặc session
        public void SaveCart(ApplicationDbContext db)
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                // Lưu giỏ hàng vào cơ sở dữ liệu cho người dùng đăng nhập
                var existingCart = db.Carts.Include("CartItems").SingleOrDefault(c => c.UserId == UserId);
                if (existingCart != null)
                {
                    db.Carts.Remove(existingCart);
                }
                Cart dbCart = new Cart
                {
                    UserId = UserId,
                    CartItems = this.items.Select(i => new CartItem
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Alias = i.Alias,
                        ProductImg = i.ProductImg,
                        CategoryName = i.CategoryName,
                        Price = i.Price,
                        Quantity = i.Quantity,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                };
                db.Carts.Add(dbCart);
                db.SaveChanges();
            }
            else
            {
                // Lưu giỏ hàng vào session cho người dùng không đăng nhập
                HttpContext.Current.Session["Cart"] = this;
            }
        }

        // Hàm tải giỏ hàng từ cơ sở dữ liệu hoặc session
        public static ShoppingCart LoadCart(string userId, ApplicationDbContext db)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var dbCart = db.Carts.Include("CartItems").SingleOrDefault(c => c.UserId == userId);
                if (dbCart != null)
                {
                    ShoppingCart cart = new ShoppingCart(userId);
                    cart.items = dbCart.CartItems.Join(
                        db.Products,
                        cartItem => cartItem.ProductId,
                        product => product.Id,
                        (cartItem, product) => new ShoppingCartItem
                        {
                            ProductId = cartItem.ProductId,
                            ProductName = cartItem.ProductName,
                            Alias = cartItem.Alias,
                            ProductImg = cartItem.ProductImg,
                            CategoryName = cartItem.CategoryName,
                            Price = product.Price,
                            Quantity = cartItem.Quantity,
                            LeftQuantity = product.Quantity,
                            TotalPrice = cartItem.TotalPrice
                        }).ToList();
                    return cart;
                }
            }
            return (ShoppingCart)HttpContext.Current.Session["Cart"] ?? new ShoppingCart();
        }
    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public int LeftQuantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
    }
}
