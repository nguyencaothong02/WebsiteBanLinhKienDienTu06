using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Admin/Product
        public ActionResult Index(string currentFilter, int? page, string SearchString)
        {
            var listProduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                // lấy danh sách sản phẩm theo từ khóa
                listProduct = qllk.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();

            }
            else
            {
                // lấy tất cả sản phẩm trong bảng product
                listProduct = qllk.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            // số lượng item của 1 trang là 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // sắp xếp theo id sản phẩm, sản phẩm mới đưa lên đầu nha cu
           // listProduct = listProduct.OrderByDescending(n => n.ProductID).ToList();

         //   var listProduct = qllk.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();
            return View(listProduct.ToPagedList(pageNumber, pageSize));
        }


        // GET: Admin/Product/Details/5
        public ActionResult Details(int id)
        {
            //Product p = da.Products.Where(q => q.ProductID == id).FirstOrDefault();
            Product p = qllk.Products.FirstOrDefault(q => q.ProductID == id);
            return View(p);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["NCC"] = new SelectList(qllk.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(qllk.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Product/Create
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product product, FormCollection collection)
        {
            try
            {

                // Tạo 1 Sp mới
                Product p = new Product();
                // Gán thuộc tính cho SP                
                p = product;
                p.CategoryID = int.Parse(collection["LSP"]);
                p.SupplierID = int.Parse(collection["NCC"]);

                // Thêm vào Product
                qllk.Products.Add(p);
                // Lưu vào db
                qllk.SaveChanges();
                // Gọi hiển thị DSSP

                return RedirectToAction("Index");
            }
            catch
            {
                return View();

            }
        }

        // Tạo 1 view chứa thông tin SP cần sửacsx
        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product p = qllk.Products.FirstOrDefault(s => s.ProductID == id);
            ViewData["NCC"] = new SelectList(qllk.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(qllk.Categories, "CategoryID", "CategoryName");
            return View(p);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product, FormCollection collection)
        {
            try
            {
                // Lấy SP muốn sửa
                Product p = qllk.Products.First(s => s.ProductID == product.ProductID);

                p.CategoryID = int.Parse(collection["LSP"]);
                p.SupplierID = int.Parse(collection["NCC"]);
                // Gán thuộc tính cho SP
                p.ProductName = product.ProductName;
                p.Price = product.Price;
                p.Quantity = product.Quantity;
                p.Description = product.Description;
                p.ImageUrl = product.ImageUrl;
                p.Discoutinued = product.Discoutinued;


                // Lưu vào db   
                qllk.SaveChanges();

                // Gọi hiển thị DSSP
                // khi nhất nút edit thì sẽ chuyển về link /Index
                return RedirectToAction("Index");
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
            Product p = qllk.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }
        // Thực hiện xóa
        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Product p = qllk.Products.First(s => s.ProductID == id);
                qllk.Products.Remove(p);

                // Lưu vào db
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