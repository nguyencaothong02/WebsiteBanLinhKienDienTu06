using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;
using System.Data.Objects.SqlClient;
using System.Data.Entity.Core.Objects;
using System.Globalization;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult GetStatistical(string fromDate, string toDate)
        //{
        //    var query = from o in qllk.Orders
        //                join od in qllk.OrderDetails
        //                on o.OrderID equals od.OrderID
        //                join p in qllk.Products
        //                on od.ProductID equals p.ProductID
        //                select new
        //                {
        //                    OrderDate = o.OrderDate,
        //                    Quantity = od.Quantity,
        //                    Price = od.Price
        //                };
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
        //        query = query.Where(x=>x.OrderDate >= startDate);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
        //        query = query.Where(x => x.OrderDate < endDate);
        //    }
        //    var result = query.GroupBy(x => DbFunctions.TruncateTime(x.OrderDate)).Select(x => new
        //    {
        //        Date = x.Key.Value,
        //        TotalSell = x.Sum(y => y.Quantity * y.Price),
        //    }).Select(x => new
        //    {
        //        Date = x.Date,
        //        DoanhThu = x.TotalSell,
        //    });
        //    return Json(new {Data = result}, JsonRequestBehavior.AllowGet);
        //}

        //Thống kê theo tháng
        [HttpGet]
        public ActionResult GetStatisticalMonth(string fromDate, string toDate)
        {
            var query = from o in qllk.Orders
                        join od in qllk.OrderDetails
                        on o.OrderID equals od.OrderID
                        join p in qllk.Products
                        on od.ProductID equals p.ProductID
                        select new
                        {
                            OrderDate = EntityFunctions.TruncateTime(o.OrderDate),
                            Quantity = od.Quantity,
                            Price = od.Price
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query = query.Where(x => x.OrderDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query = query.Where(x => x.OrderDate < endDate.AddDays(1));
            }

            var result = query.AsEnumerable().GroupBy(x => new
            {
                Year = x.OrderDate.Value.Year,
                Month = x.OrderDate.Value.Month
            })
                .Select(x => new
                {
                    Month = $"{x.Key.Month}/{x.Key.Year}",
                    TotalSell = x.Sum(y => y.Quantity * y.Price)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


        // Thống kê theo ngày
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in qllk.Orders
                        join od in qllk.OrderDetails
                        on o.OrderID equals od.OrderID
                        join p in qllk.Products
                        on od.ProductID equals p.ProductID
                        select new
                        {
                            OrderDate = EntityFunctions.TruncateTime(o.OrderDate),
                            Quantity = od.Quantity,
                            Price = od.Price
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate;
                CultureInfo culture = new CultureInfo("vi-VN");
                DateTimeFormatInfo dtfi = culture.DateTimeFormat.Clone() as DateTimeFormatInfo;
                dtfi.Calendar = new GregorianCalendar();
                if (DateTime.TryParseExact(fromDate, "dd/MM/yyyy", culture, DateTimeStyles.None, out startDate))
                {
                    query = query.Where(x => x.OrderDate >= startDate);
                }
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate;
                CultureInfo culture = new CultureInfo("vi-VN");
                DateTimeFormatInfo dtfi = culture.DateTimeFormat.Clone() as DateTimeFormatInfo;
                dtfi.Calendar = new GregorianCalendar();
                if (DateTime.TryParseExact(toDate, "dd/MM/yyyy", culture, DateTimeStyles.None, out endDate))
                {
                    query = query.Where(x => x.OrderDate < endDate.AddDays(1));
                }
            }


            var result = query.AsEnumerable() // theo ngày tăng dần
                .GroupBy(x => x.OrderDate.Value.Date)
                .Select(x => new { Day = x.Key.ToString("dd/MM/yyyy"), TotalSell = x.Sum(y => y.Quantity * y.Price) })
                .OrderBy(x => x.Day).ToList();



            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}