using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Detail(int id)
        {
            var product = db.Products.Include("ReviewProducts")
                             .FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var averageRating = product.ReviewProducts.Any()
                ? product.ReviewProducts.Average(x => x.Rate)
                : 0;
            ViewBag.AverageRating = averageRating;
            var countReview = db.ReviewProducts.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;
            return View(product);
        }

        public ActionResult ProductCategory(string alias, float? priceMin, float? priceMax, int? id, int page = 1)
        {
            int pageSize = 8;
            IEnumerable<Product> items = db.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.ProductCategory.Id == id);
                var cate = db.ProductCategories.Find(id);
                ViewBag.CateName = cate?.Title;
                ViewBag.Alias = alias;
            }
            else
            {
                ViewBag.Alias = "tat-ca";
            }
            ViewBag.CateId = id;

            if (priceMin.HasValue)
            {
                items = items.Where(x => x.Price >= priceMin.Value);
                ViewBag.PriceMin = priceMin;
            }
            if (priceMax.HasValue)
            {
                items = items.Where(x => x.Price <= priceMax.Value);
                ViewBag.PriceMax = priceMax;
            }

            var pagedItems = items.ToPagedList(page, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Partial_ProductCategory", pagedItems);
            }
            return View(pagedItems);
        }

        public ActionResult Partial_ProductByCateId()
        {
            var items = db.Products.Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var query = from p in db.Products
                        join od in db.OrderDetails on p.Id equals od.ProductId into productSales
                        from od in productSales.DefaultIfEmpty()
                        where p.IsActive
                        group od by new
                        {
                            p.Id,
                            p.Title,
                            p.Quantity,
                            p.Price,
                            p.Alias
                        } into g
                        select new
                        {
                            ProductId = g.Key.Id,
                            ProductName = g.Key.Title,
                            SoldQuantity = g.Sum(x => x == null ? 0 : x.Quantity), // Tổng số lượng đã bán
                            ProductPrice = g.Key.Price,
                            ProductAlias = g.Key.Alias,
                            RemainingQuantity = g.Key.Quantity // Số lượng còn lại trong kho
                        };

            var topProducts = query.OrderByDescending(x => x.SoldQuantity).Take(8).ToList();

            var productList = new List<Product>();

            foreach (var topProduct in topProducts)
            {
                var images = db.ProductImages.Where(x => x.ProductId == topProduct.ProductId).ToList();
                var defaultImage = images.FirstOrDefault(x => x.IsDefault)?.Image ?? "/Uploads/images/No_Image_Available.jpg";
                var hoverImage = images.FirstOrDefault(x => x.IsHover)?.Image ?? "/Uploads/images/No_Image_Available.jpg";
                var product = new Product
                {
                    Id = topProduct.ProductId,
                    Title = topProduct.ProductName,
                    Quantity = topProduct.RemainingQuantity,
                    ProductImage = new List<ProductImage>
                    {
                        new ProductImage { Image = defaultImage, IsDefault = true },
                        new ProductImage { Image = hoverImage, IsHover = true }
                    },
                    Price = topProduct.ProductPrice,
                    Alias = topProduct.ProductAlias,
                };
                productList.Add(product);
            }
            return PartialView(productList);
        }

        public ActionResult Partial_ProductRelated(int categoryId, int productId)
        {
            var items = db.Products
                .Where(x => x.ProductCategoryId == categoryId && x.Id != productId && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return PartialView(items);
        }

        [HttpGet]
        public JsonResult SearchProducts(string searchTerm)
        {
            var normalizedSearchTerm = NormalizeVietnamese(searchTerm.ToLower());

            var products = db.Products
                .AsEnumerable()
                .Where(p => NormalizeVietnamese(p.Title.ToLower()).Contains(normalizedSearchTerm))
                .Select(p => new { p.Alias, p.Id, p.Title })
                .ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        private string NormalizeVietnamese(string input)
        {
            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Replace special characters with their normalized form
            stringBuilder.Replace("đ", "d"); // Replace 'đ' with 'd'
            stringBuilder.Replace("Đ", "D"); // Replace 'Đ' with 'D'
                                             // Add more replacements if necessary

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}