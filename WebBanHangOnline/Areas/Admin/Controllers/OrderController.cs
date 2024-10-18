using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int page = 1)
        {
            var pageSize = 10;
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            var pagedList = items.ToPagedList(page, pageSize);            
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            if (Request.IsAjaxRequest())
            {
                return PartialView(pagedList);
            }
            return View(pagedList);
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_ItemsOrdered(int id)
        {
            var item = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = db.Orders.Find(id);
            if (item != null)
            {
                db.Orders.Attach(item);
                item.TypePayment = trangthai;
                db.Entry(item).Property(x => x.TypePayment).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", success = true });
            }
            return Json(new { message = "Unsuccess", success = false });
        }
    }
}