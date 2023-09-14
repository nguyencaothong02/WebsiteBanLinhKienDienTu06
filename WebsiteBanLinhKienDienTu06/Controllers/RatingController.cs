using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class RatingController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Rating
        public ActionResult Index()
        {
            var listRating = qllk.Ratings.ToList();
            return View(listRating);
        }

        // Hàm kiểm tra xem khách hàng đã mua sản phẩm hay chưa
        //private bool CheckIfCustomerPurchasedProduct(string customerId, int productId)
        //{
        //    // Thực hiện truy vấn cơ sở dữ liệu để kiểm tra xem khách hàng đã mua sản phẩm hay chưa
        //    bool hasPurchased = qllk.OrderDetails
        //        .Any(od => od.Order.CustomerID == customerId && od.ProductID == productId);

        //    return hasPurchased;
        //}

        private int GetPurchasedProductCount(string customerId, int productId)
        {
            int purchasedCount = qllk.OrderDetails
                .Count(od => od.Order.CustomerID == customerId && od.ProductID == productId);

            return purchasedCount;
        }

        public ActionResult ProductRating(int id)
        {
            //ViewBag.ProductID = id;
            ////ViewBag.ProductID = qllk.OrderDetails.Where(p => p.ProductID == id);
            //var listProductRating = qllk.Ratings.Where(r => r.ProductID == id).ToList();
            //return View(listProductRating);
            var listProductRating = qllk.Ratings.Where(r => r.ProductID == id).ToList();

            //Code đúng nha
            if (Session["idCustomer"] != null)
            {
                string customerId = Session["idCustomer"].ToString();
                int hasPurchased = GetPurchasedProductCount(customerId, id);

                ViewBag.HasPurchased = hasPurchased > 0;
                ViewBag.ProductID = id;

                //listProductRating = qllk.Ratings.Where(r => r.ProductID == id).ToList();
                
            }
            return View(listProductRating);
            //else
            //{
            //    // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
            //    return RedirectToAction("Login", "Home");
            //}

            //if (Session["idCustomer"] != null || Session["idEmployee"] != null)
            //{
            //    string customerId = Session["idCustomer"]?.ToString();
            //    string employeeId = Session["idEmployee"]?.ToString();

            //    if (customerId != null || employeeId != null)
            //    {
            //        int hasPurchased = GetPurchasedProductCount(customerId, id);

            //        ViewBag.HasPurchased = hasPurchased > 0;
            //        ViewBag.ProductID = id;

            //        var listProductRating = qllk.Ratings.Where(r => r.ProductID == id).ToList();
            //        return View(listProductRating);
            //    }
            //}

            //// Người dùng chưa đăng nhập hoặc không phải là nhân viên, chuyển hướng đến trang đăng nhập
            //return RedirectToAction("Login", "Home");

        }



        public ActionResult Create(int id)
        {
            var rating = new Rating { ProductID = id };
            rating.CustomerID = Session["idCustomer"].ToString();
            return View(rating);
        }

        // POST: Admin/Product/Create
        [HttpPost]
        public ActionResult Create(Rating rating)
        {
            try
            {
                
                Rating r = rating;
                r.CreatedDate = DateTime.Now;

                // Lấy thông tin sản phẩm từ database để thiết lập thuộc tính navigation
                var product = qllk.Products.FirstOrDefault(p => p.ProductID == r.ProductID);
                r.Product = product;

                // Lấy thông tin khách hàng từ session để thiết lập thuộc tính navigation
                r.CustomerID = Session["idCustomer"].ToString();
                var customer = qllk.Customers.FirstOrDefault(c => c.CustomerID == r.CustomerID);
                r.Customer = customer;

                qllk.Ratings.Add(r);
                qllk.SaveChanges();

                return RedirectToAction("ProductRating", new { id = rating.ProductID });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while creating the rating: " + ex.Message;
                return View(rating);
            }

        }

        public ActionResult Edit(int id)
        {
            Rating r = qllk.Ratings.FirstOrDefault(s => s.ProductID == id);
            return View(r);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Rating rating, FormCollection collection)
        {
            try
            {
                // Lấy SP muốn sửa
                Rating r = qllk.Ratings.First(s => s.ProductID == rating.ProductID);
                // Gán thuộc tính cho SP
                r.Score = rating.Score;
                r.Comment = rating.Comment;
                r.CreatedDate = DateTime.Now;

                // Lưu vào db   
                qllk.SaveChanges();

                // Gọi hiển thị DSSP
                // khi nhất nút edit thì sẽ chuyển về link /Index
                return RedirectToAction("ProductRating", new { id = rating.ProductID });

            }
            catch
            {
                return View();
            }
        }



        // View SP muốn xóa
        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            Rating r = qllk.Ratings.FirstOrDefault(s => s.ProductID == id);
            return View(r);
        }
        // Thực hiện xóa
        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Rating r = qllk.Ratings.First(s => s.ProductID == id);
                qllk.Ratings.Remove(r);

                // Lưu vào db
                qllk.SaveChanges();
                return RedirectToAction("ProductRating", new { id = r.ProductID });

            }
            catch
            {
                return View();
            }
        }

        // GET: Product/DeleteAdmin/5
        public ActionResult DeleteAdmin(string id)
        {
            Rating r = qllk.Ratings.FirstOrDefault(s => s.CustomerID == id);
            return View(r);
        }
        // Thực hiện xóa
        // POST: Product/DeleteAdmin/5
        [HttpPost]
        public ActionResult DeleteAdmin(string id, FormCollection collection)
        {
            try
            {
                Rating r = qllk.Ratings.First(s => s.CustomerID == id);
                qllk.Ratings.Remove(r);

                // Lưu vào db
                qllk.SaveChanges();
                return RedirectToAction("ProductRating", new { id = r.ProductID });

            }
            catch
            {
                return View();
            }
        }
    }
}