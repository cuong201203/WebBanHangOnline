using ClientApp.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using PagedList;

namespace WebBanHangOnline.Controllers
{
    [CustomAuthorize("/account/login")]
    public class ReviewProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ReviewProduct
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if (user != null)
                {
                    item.Email = user.Email;
                    item.FullName = user.FullName;
                    item.UserName = user.UserName;
                }
                return PartialView(item);
            }
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult _LoadReview(int productId, int? page)
        {
            var pageSize = 5; // Số lượng đánh giá mỗi trang
            var pageIndex = page ?? 1; // Trang hiện tại
            var reviews = db.ReviewProducts
                             .Where(x => x.ProductId == productId)
                             .OrderByDescending(x => x.CreatedDate)
                             .ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageIndex;
            ViewBag.ProductId = productId; // Truyền productId cho các yêu cầu phân trang

            return PartialView(reviews);
        }

        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if (ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                db.ReviewProducts.Add(req);
                db.SaveChanges();

                // Tính toán điểm đánh giá trung bình
                var averageRating = db.ReviewProducts
                    .Where(x => x.ProductId == req.ProductId)
                    .Average(x => x.Rate); // Giả sử có thuộc tính Rating trong ReviewProduct

                // Đếm số lượng đánh giá
                var countReview = db.ReviewProducts
                    .Count(x => x.ProductId == req.ProductId);

                return Json(new { success = true, averageRating, countReview });
            }
            return Json(new { success = false });
        }

    }
}