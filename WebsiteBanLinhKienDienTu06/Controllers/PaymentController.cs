using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;
using WebsiteBanLinhKienDienTu06.Models;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class PaymentController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();

        // GET: Payment
        public ActionResult Index()
        {
            decimal? totalPrice = 0;
            string numrd;
            Random rd = new Random();
            numrd = rd.Next(1, 6).ToString();
            if (Session["idCustomer"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                // Lấy thông tin từ giỏ hàng từ biến session
                var listCart = (List<CartModel>)Session["cart"];
                // Gán dữ liệu cho Order
                Order o = new Order();
                o.CustomerID = Session["idCustomer"].ToString();
                o.OrderDate = DateTime.Now;
                o.ShippingAddress = Session["Address"].ToString();
                o.ContactName = Session["UserName"].ToString();
                o.ContactPhone = Session["Phone"].ToString();
                o.EmployeeID = int.Parse(numrd);
                qllk.Orders.Add(o);
                // Lưu thông tin vào bảng Order
                qllk.SaveChanges();

                // Lấy OrderID vừa mới tạo vào bảng OrderDetail
                int orderid = o.OrderID;
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var item in listCart)
                {
                    OrderDetail od = new OrderDetail();
                    decimal? price = item.Product.Price;
                    var total = price * item.Quantity;
                    totalPrice += total; // Cộng dồn tổng tiền từng sản phẩm vào totalPrice
                    od.Quantity = item.Quantity;
                    od.OrderID = orderid;
                    od.ProductID = item.Product.ProductID;
                    od.Price = total;
                    od.Discount = 0;
                    orderDetails.Add(od);
                }
                qllk.OrderDetails.AddRange(orderDetails);
                qllk.SaveChanges();

                // Tạo đối tượng InvoiceModel và gán giá trị
                InvoiceModel invoice = new InvoiceModel();
                invoice.OrderID = o.OrderID;
                invoice.CustomerID = o.CustomerID;
                invoice.OrderDate = (DateTime)o.OrderDate;
                invoice.ShippingAddress = o.ShippingAddress;
                invoice.ContactName = o.ContactName;
                invoice.ContactPhone = o.ContactPhone;
                invoice.TotalPrice = (decimal)totalPrice;

                TempData["Invoice"] = invoice;
            }
            



            return View();
        }
        public ActionResult ViewInvoice()
        {
            // Lấy đối tượng InvoiceModel từ TempData
            InvoiceModel invoice = TempData["Invoice"] as InvoiceModel;

            if (invoice == null)
            {
                // Nếu không có đối tượng InvoiceModel trong TempData, điều hướng người dùng đến trang lỗi hoặc trang thông báo không tìm thấy hóa đơn.
                return RedirectToAction("Error", "Home");
            }

            return View(invoice);
        }
    }
}