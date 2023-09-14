using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;
using WebsiteBanLinhKienDienTu06.Models;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class ProductController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Product
        public ActionResult Detail(int id)
        {
            var detailProduct = qllk.Products.Where(p => p.ProductID == id).FirstOrDefault();

            var comments = qllk.Ratings.Where(r => r.ProductID == id).ToList();

            ViewBag.Comments = comments;
            ViewBag.ProductId = id;


            return View(detailProduct);
        }

        // POST: Product/Comment
        [HttpPost]
        public ActionResult Comment(CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var rating = new Rating
                {
                    ProductID = comment.ProductID,
                    CustomerID = comment.CustomerID,
                    Score = comment.Score,
                    Comment = comment.Comment,
                    CreatedDate = DateTime.Now
                };

                qllk.Ratings.Add(rating);
                qllk.SaveChanges();

                return RedirectToAction("Details", new { id = comment.ProductID });
            }

            // Nếu dữ liệu không hợp lệ, quay trở lại trang sản phẩm
            return RedirectToAction("Details", new { id = comment.ProductID });
        }
    }
}