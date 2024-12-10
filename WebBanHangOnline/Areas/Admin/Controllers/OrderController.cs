using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(string searchText, string fromDate, string toDate, int page = 1)
        {
            var pageSize = 10;           

            var items = db.Orders.OrderByDescending(x => x.CreatedDate).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Code.Contains(searchText) ||
                                         x.CustomerName.Contains(searchText) ||
                                         x.Phone.Contains(searchText) ||
                                         x.CreatedBy.Contains(searchText));
            }

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                DateTime endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", null);
                items = items.Where(x => DbFunctions.TruncateTime(x.CreatedDate) >= startDate && DbFunctions.TruncateTime(x.CreatedDate) <= endDate);
            }

            var pagedList = items.ToPagedList(page, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.SearchText = searchText;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            return View(pagedList);
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult ItemsOrdered(int id)
        {
            var item = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView("_ItemsOrdered", item);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.Status = status;
                db.Entry(item).Property(x => x.Status).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", success = true });
            }
            return Json(new { message = "Unsuccess", success = false });
        }
    }
}