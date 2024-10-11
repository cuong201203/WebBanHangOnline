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
        // GET: Product
        public ActionResult Index(int? id)
        {
            var items = db.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            return View(items);
        }

        public ActionResult Detail(string alias, int id)
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

        public ActionResult ProductCategory(string alias, int? id)
        {
            var items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).OrderByDescending(x => x.CreatedDate).Take(10).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_RelatedProducts(int categoryId, int productId)
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