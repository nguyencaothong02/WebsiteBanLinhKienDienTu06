using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;
using WebsiteBanLinhKienDienTu06.Models;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class HomeController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        public JsonResult IsUsernameUnique(string Username)
        {
            bool isUnique = !qllk.Customers.Any(c => c.Username == Username);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            // Tạo 1 đối tượng HomeModel chứa 2 cái danh sách: sản phẩm và loại sản phẩm
            HomeModel homeModel = new HomeModel();
            
            homeModel.ListCategory = qllk.Categories.ToList();
            homeModel.ListProduct = qllk.Products.ToList();
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }
            return View(homeModel);
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

        [HttpGet]
        public ActionResult Register()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer, FormCollection collection)
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
        //hàm mã hóa password
        //public static string GetMD5(string str)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] fromData = Encoding.UTF8.GetBytes(str);
        //    byte[] targetData = md5.ComputeHash(fromData);
        //    string byte2String = null;

        //    for (int i = 0; i < targetData.Length; i++)
        //    {
        //        byte2String += targetData[i].ToString("x2");

        //    }
        //    return byte2String;
        //}
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                //var f_password = GetMD5(password);
                var user = qllk.Customers.FirstOrDefault(c => c.Email == email);
                // Kiểm tra thông tin đăng nhập của nhân viên
                
                var employee = qllk.Employees.Where(s => s.Email.Equals(email) && s.Password.Equals(password)).ToList();
                var customer = qllk.Customers.Where(s => s.Email.Equals(email) && s.Password.Equals(password)).ToList();

                if (customer.Count() > 0)
                {
                    //add session
                    Session["FullName"] = customer.FirstOrDefault().FirstName + " " + customer.FirstOrDefault().LastName;
                    Session["Email"] = customer.FirstOrDefault().Email;
                    Session["idCustomer"] = customer.FirstOrDefault().CustomerID;
                    Session["UserName"] = customer.FirstOrDefault().Username;
                    Session["Address"] = customer.FirstOrDefault().Address;
                    Session["Phone"] = customer.FirstOrDefault().PhoneNumber;
                    ViewBag.User = user.Username;
                    return RedirectToAction("Index");
                    
                }
                else if (employee.Count() > 0)
                {
                    //add session
                    Session["FullName"] = employee.FirstOrDefault().FirstName + " " + employee.FirstOrDefault().LastName;
                    Session["Email"] = employee.FirstOrDefault().Email;
                    Session["idEmployee"] = employee.FirstOrDefault().EmployeeID;
                    Session["UserName"] = employee.FirstOrDefault().Username;
                    Session["Address"] = employee.FirstOrDefault().Address;
                    Session["Phone"] = employee.FirstOrDefault().PhoneNumber;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}