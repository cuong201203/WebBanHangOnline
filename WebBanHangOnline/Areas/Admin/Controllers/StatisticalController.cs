using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate, string viewMode)
        {
            DateTime startDate;
            DateTime endDate;

            if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
            {
                endDate = DateTime.Now.Date;
                startDate = endDate.AddDays(-14); // 15 ngày gần nhất
            }
            else
            {
                startDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        where DbFunctions.TruncateTime(o.CreatedDate) >= startDate && DbFunctions.TruncateTime(o.CreatedDate) <= endDate
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice,
                        };

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                Revenue = x.TotalSell,
                Profit = x.TotalSell - x.TotalBuy,
            }).OrderBy(x => x.Date).ToList(); // Sort by date ascending

            // Fill in missing dates with zero revenue and profit
            var dateRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                      .Select(offset => startDate.AddDays(offset))
                                      .ToList();

            var finalResult = from date in dateRange
                              join data in result on date equals data.Date into gj
                              from subData in gj.DefaultIfEmpty()
                              select new
                              {
                                  Date = date,
                                  Revenue = subData?.Revenue ?? 0,
                                  Profit = subData?.Profit ?? 0
                              };

            // Group by viewMode
            var groupedResult = GroupByMode(finalResult, viewMode);

            return Json(new { Data = groupedResult }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<dynamic> GroupByMode(IEnumerable<dynamic> data, string viewMode)
        {
            switch (viewMode)
            {
                case "month":
                    return data.GroupBy(d => new { d.Date.Year, d.Date.Month })
                               .Select(g => new
                               {
                                   Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                                   Revenue = g.Sum(d => d.Revenue),
                                   Profit = g.Sum(d => d.Profit)
                               })
                               .OrderBy(d => d.Date); // Sort by date ascending

                case "year":
                    return data.GroupBy(d => d.Date.Year)
                               .Select(g => new
                               {
                                   Date = new DateTime(g.Key, 1, 1),
                                   Revenue = g.Sum(d => d.Revenue),
                                   Profit = g.Sum(d => d.Profit)
                               })
                               .OrderBy(d => d.Date); // Sort by date ascending

                case "day":
                default:
                    return data;
            }
        }
    }
}
