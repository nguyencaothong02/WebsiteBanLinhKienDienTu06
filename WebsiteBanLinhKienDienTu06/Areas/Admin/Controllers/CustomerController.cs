using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Admin/Customer
        public ActionResult Index(string currentFilter, int? page, string SearchString)
        {
            var listCustomer = new List<Customer>();
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
                // lấy danh sách khách hàng theo từ khóa
                listCustomer = qllk.Customers.Where(p => p.Username.Contains(SearchString)).ToList();

            }
            else
            {
                // lấy tất cả khách hàng trong bảng product
                listCustomer = qllk.Customers.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            // số lượng item của 1 trang là 4
            int pageSize = 4;
            
            int pageNumber = (page ?? 1);
            // sắp xếp theo id khách hàng, khách hàng mới đưa lên đầu nha cu
            // listProduct = listProduct.OrderByDescending(n => n.ProductID).ToList();

            //   var listProduct = qllk.Products.Where(p => p.ProductName.Contains(SearchString)).ToList();
            return View(listCustomer.ToPagedList(pageNumber, pageSize));
        }

        // hàm tăng giá trị tự động cho CustomerID
        private string GenerateNextCustomerID(string lastCustomerID)
        {
            // Tạo giá trị tăng dần mới dựa trên giá trị CustomerID lớn nhất hiện tại
            // Ví dụ: Nếu lastCustomerID là "CUS03", giá trị mới sẽ là "CUS04"

            // Cài đặt logic tạo giá trị tăng dần phù hợp với yêu cầu của bạn
            // Ví dụ:
            int currentID = int.Parse(lastCustomerID.Replace("CUS", ""));
            int nextID = currentID + 1;
            string newCustomerID = "CUS" + nextID.ToString("D2");

            return newCustomerID;
        }

        // GET: KhachHang/Create
        public ActionResult Create()
        {
            List<SelectListItem> genderList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = "Male" },
                new SelectListItem { Text = "Female", Value = "Female" },
                new SelectListItem { Value = "Other", Text = "N/A" }
            };
            ViewBag.GenderList = genderList;
            return View();
        }

        // POST: KhachHang/Create
        [HttpPost]
        public ActionResult Create(Customer customer, FormCollection collection)
        {
            // Lấy giá trị CustomerID lớn nhất trong cơ sở dữ liệu
            string lastCustomerID = qllk.Customers.Select(c => c.CustomerID).Max();
            // Tạo giá trị tăng dần mới
            string newCustomerID = GenerateNextCustomerID(lastCustomerID);
            try
            {
                Customer cus = new Customer();
                // kiểm tra có đúng với các thuộc tính đã nhập trong db hay không
                // Vd nếu CustomerID nvarchar(5) mà nhập aaaaaa (6 ký tự thì sai)
                if (ModelState.IsValid)
                {
                    var checkEmail = qllk.Customers.FirstOrDefault(c => c.Email == customer.Email);
                    if (checkEmail == null)
                    {
                        var checkUser = qllk.Customers.FirstOrDefault(c => c.Username == customer.Username);
                        if (checkUser == null)
                        {
                            cus = customer;
                            // mã hóa password
                            //cus.Password = GetMD5(customer.Password);
                            cus.CustomerID = newCustomerID;
                            cus.Gender = collection["Gender"]; // Lấy giá trị được chọn từ DropDownList "Gender"
                                                               // Tắt chức năng xác thực tự động này trước khi lưu thay đổi vào cơ sở dữ liệu.
                            qllk.Configuration.ValidateOnSaveEnabled = false;
                            qllk.Customers.Add(cus);
                            qllk.SaveChanges();
                            return RedirectToAction("Index"); 
                        }
                       
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View();
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return View();
        }


        // GET: Admin/Customer/Details/5
        public ActionResult Details(string id)
        {
            //Product p = da.Products.Where(q => q.ProductID == id).FirstOrDefault();
            Customer c = qllk.Customers.FirstOrDefault(q => q.CustomerID == id);
            return View(c);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(string id)
        {
            Customer c = qllk.Customers.FirstOrDefault(s => s.CustomerID == id);
            return View(c);
        }
        // Thực hiện xóa
        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                Customer c = qllk.Customers.First(s => s.CustomerID == id);
                qllk.Customers.Remove(c);

                // Lưu vào db
                qllk.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            catch
            {
                return View();
            }
        }
    }
}