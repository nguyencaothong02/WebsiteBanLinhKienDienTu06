using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
   
    public class CategoryController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Category
        public ActionResult Index()
        {

            var listCategory = qllk.Categories.ToList();
            

            return View(listCategory);
        }
        public ActionResult ProductCaterogy(int id)
        {
            
            var listProduct = qllk.Products.Where(p => p.CategoryID == id).ToList();
            

            // Lưu số lượng sản phẩm vào biến Session
            
            return View(listProduct);
        }    
    }
}