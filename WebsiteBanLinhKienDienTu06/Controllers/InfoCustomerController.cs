using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class InfoCustomerController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: InfoCustomer
        public ActionResult Index()
        {
            return View();

        }

        // GET: InfoCustomer/Details/
        public ActionResult Details(string id)
        {
           
            //Product p = da.Products.Where(q => q.ProductID == id).FirstOrDefault();
            Customer c = qllk.Customers.FirstOrDefault(q => q.CustomerID == id);

            return View(c);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string id)
        {
            Customer c = qllk.Customers.FirstOrDefault(s => s.CustomerID == id);
            return View(c);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Customer customer, FormCollection collection)
        {
            try
            {
                // Lấy SP muốn sửa
                Customer cus = qllk.Customers.First(s => s.CustomerID == customer.CustomerID);
                // Gán thuộc tính cho SP
                cus.Username = customer.Username;
                cus.Password = customer.Password;
                cus.LastName = customer.LastName;
                cus.FirstName = customer.FirstName;
                cus.Gender = customer.Gender;
                cus.DateOfBirth = customer.DateOfBirth;
                cus.Address = customer.Address;
                cus.Email = customer.Email;
                cus.PhoneNumber = customer.PhoneNumber;
                // Lưu vào db   
                qllk.SaveChanges();
                // Gọi hiển thị DSSP
                // Chuyển hướng về trang /Home/Index
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }
    }
}