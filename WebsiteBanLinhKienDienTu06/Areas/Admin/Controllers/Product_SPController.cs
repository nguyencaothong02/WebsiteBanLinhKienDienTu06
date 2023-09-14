using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class Product_SPController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();

        // GET: Admin/Product_SP
        public ActionResult Index(string currentFilter, int? page, string SearchString)
        {
            List<Product> products = new List<Product>();
            int pageSize = 0;
            int pageNumber = 1;
            using (var context = new QuanLyLinhKienDienTuEntities())
            {
                products = context.Database
                    .SqlQuery<Product>("EXEC GetProducts")
                    .ToList();

                foreach (var product in products)
                {
                    product.Category = context.Categories
                        .FirstOrDefault(c => c.CategoryID == product.CategoryID);

                    product.Supplier = context.Suppliers
                        .FirstOrDefault(s => s.SupplierID == product.SupplierID);
                }
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
                    products = qllk.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();

                }
                else
                {
                    // lấy tất cả sản phẩm trong bảng product
                    products = qllk.Products.ToList();
                }
                ViewBag.CurrentFilter = SearchString;
                // số lượng item của 1 trang là 4
                pageSize = 4;
                pageNumber = (page ?? 1);
                // sắp xếp theo id sản phẩm, sản phẩm mới đưa lên đầu nha cu
                // listProduct = listProduct.OrderByDescending(n => n.ProductID).ToList();
            }

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryNames = new SelectList(qllk.Categories, "CategoryID", "CategoryName");
            ViewBag.CompanyNames = new SelectList(qllk.Suppliers, "SupplierID", "CompanyName");
            return View();

        }

        // POST: Admin/Product/Create

        [HttpPost]
        public ActionResult Create(Product product, FormCollection collection)
        {
            using (var context = new QuanLyLinhKienDienTuEntities())
            {
                // trans nè
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var productNameParameter = new SqlParameter("@ProductName", product.ProductName);
                        var categoryIDParameter = new SqlParameter("@CategoryID", SqlDbType.Int);
                        categoryIDParameter.Value = (object)product.CategoryID ?? DBNull.Value;
                        var supplierIDParameter = new SqlParameter("@SupplierID", SqlDbType.Int);
                        supplierIDParameter.Value = (object)product.SupplierID ?? DBNull.Value;
                        var priceParameter = new SqlParameter("@Price", product.Price);
                        var quantityParameter = new SqlParameter("@Quantity", product.Quantity);
                        var descriptionParameter = new SqlParameter("@Description", product.Description);
                        var imageUrlParameter = new SqlParameter("@ImageUrl", product.ImageUrl);
                        var discoutinuedParameter = new SqlParameter("@Discoutinued", product.Discoutinued);

                        var categoryNameParameter = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
                        categoryNameParameter.Direction = ParameterDirection.Output;

                        var companyNameParameter = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 40);
                        companyNameParameter.Direction = ParameterDirection.Output;

                        context.Database.ExecuteSqlCommand("EXEC CreateProduct @ProductName, @CategoryID, @SupplierID, @Price, @Quantity, @Description, @ImageUrl, @Discoutinued, @CategoryName OUT, @CompanyName OUT",
                            productNameParameter, categoryIDParameter, supplierIDParameter, priceParameter, quantityParameter, descriptionParameter, imageUrlParameter, discoutinuedParameter, categoryNameParameter, companyNameParameter);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                        // Xử lý lỗi (hiển thị thông báo, ghi log, v.v.)
                        // ...
                        return RedirectToAction("Error");
                    }
                }
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product p = qllk.Products.FirstOrDefault(s => s.ProductID == id);
            ViewBag.CategoryNames = new SelectList(qllk.Categories, "CategoryID", "CategoryName");
            ViewBag.CompanyNames = new SelectList(qllk.Suppliers, "SupplierID", "CompanyName");
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var context = new QuanLyLinhKienDienTuEntities())
            {
                // Lấy thông tin sản phẩm từ Viewproduct
                var productId = product.ProductID;
                var productName = product.ProductName;
                var categoryId = product.CategoryID;
                var supplierId = product.SupplierID;
                var price = product.Price;
                var quantity = product.Quantity;
                var description = product.Description;
                var imageUrl = product.ImageUrl;
                var discoutinued = product.Discoutinued;

                // Tạo các tham số đầu ra
                var categoryNameParam = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50);
                categoryNameParam.Direction = ParameterDirection.Output;

                var companyNameParam = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 40);
                companyNameParam.Direction = ParameterDirection.Output;

                // Gọi Stored Procedure để cập nhật dữ liệu và lấy CategoryName và CompanyName
                context.Database.ExecuteSqlCommand("UpdateProduct @ProductId, @ProductName, @CategoryId, @SupplierId, @Price, @Quantity, @Description, @ImageUrl, @Discoutinued, @CategoryName OUT, @CompanyName OUT",
                    new SqlParameter("@ProductId", productId),
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@SupplierId", supplierId),
                    new SqlParameter("@Price", price),
                    new SqlParameter("@Quantity", quantity),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@ImageUrl", imageUrl),
                    new SqlParameter("@Discoutinued", discoutinued),
                    categoryNameParam,
                    companyNameParam);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            //Product p = da.Products.Where(q => q.ProductID == id).FirstOrDefault();
            Product p = qllk.Products.FirstOrDefault(q => q.ProductID == id);
            return View(p);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = qllk.Products.FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);


        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (var context = new QuanLyLinhKienDienTuEntities())
            {
                // Gọi Stored Procedure để xóa dữ liệu
                context.Database.ExecuteSqlCommand("DeleteProduct @ProductId",
                    new SqlParameter("@ProductId", id));
            }

            return RedirectToAction("Index");
        }
    }
}