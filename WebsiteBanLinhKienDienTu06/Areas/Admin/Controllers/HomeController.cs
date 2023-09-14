using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var countOrder = qllk.Orders.Count();
            ViewBag.CountOrder = countOrder;

            var countCustomer = qllk.Customers.Count();
            ViewBag.CountCustomer = countCustomer;

            var countProduct = qllk.Products.Count();
            ViewBag.CountProduct = countProduct;

            var totalPrice = qllk.OrderDetails.Sum(od => od.Quantity * od.Price);
            string formattedPrice = String.Format("{0:#,##0} ₫", totalPrice);
            ViewBag.TotalPrice = formattedPrice;
            return View();
        }

        public ActionResult Index_TT()
        {
            return View();
        }

    }
}