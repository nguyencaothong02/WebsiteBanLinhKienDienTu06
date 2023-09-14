using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Admin/Order
        public ActionResult Index()
        {
            List<Order> orders = new List<Order>();
            orders = qllk.Orders.ToList();
            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            Order o = new Order();
            o = qllk.Orders.FirstOrDefault(q => q.OrderID == id);
            return View(o);
        }
        public ActionResult Order_Details_ID(int id)
        {
            List<OrderDetail> od = new List<OrderDetail>();
            //od = da.OrderDetails.ToList();
            od = qllk.OrderDetails.Where(a => a.OrderID == id).ToList();
            return View(od);
        }
        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(Order order, FormCollection collection)
        {
            try
            {
                Order o = new Order();
                o = order;
                qllk.Orders.Add(o);
                qllk.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            Order o = qllk.Orders.FirstOrDefault(p => p.OrderID == id);
            return View(o);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult Edit(Order order, FormCollection collection)
        {
            try
            {
                Order o = qllk.Orders.First(p => p.OrderID == order.OrderID);
                o.CustomerID = order.CustomerID;
                o.EmployeeID = order.EmployeeID;
                o.OrderDate = order.OrderDate;
                o.RequireDate = order.RequireDate;
                o.ShippedDate = order.ShippedDate;
                o.ShipVia = order.ShipVia;
                o.Freight = order.Freight;
                o.ShippingAddress = order.ShippingAddress;
                o.ContactName = order.ContactName;
                o.ContactPhone = order.ContactPhone;
                qllk.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DonHang/Delete/5
        public ActionResult Delete(int id)
        {
            Order o = qllk.Orders.FirstOrDefault(s => s.OrderID == id);
            return View(o);
        }

        // POST: DonHang/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Order o = qllk.Orders.First(s => s.OrderID == id);
                qllk.Orders.Remove(o);
                qllk.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
