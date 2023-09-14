using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanLinhKienDienTu06.Context;
using WebsiteBanLinhKienDienTu06.Models;

namespace WebsiteBanLinhKienDienTu06.Controllers
{
    public class CartController : Controller
    {
        QuanLyLinhKienDienTuEntities qllk = new QuanLyLinhKienDienTuEntities();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = (List<CartModel>)Session["cart"];
            ViewBag.IsEmptyCart = (cart == null || cart.Count == 0);
            return View(cart);

        }

        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["cart"] == null)
            {
                var Price = qllk.Products.FirstOrDefault(q => q.ProductID == id);
                List<CartModel> cart = new List<CartModel>();  
                cart.Add(new CartModel { Product = qllk.Products.Find(id), Quantity = quantity  });
                CartModel c = new CartModel();
                
                c.UnitPrice = Price.Price;
                ViewBag.Total = cart.Sum(p => p.Total);
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                List<CartModel> cart = (List<CartModel>)Session["cart"];
                //kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa???
                int index = isExist(id);
                if (index != -1)
                {
                    //nếu sp tồn tại trong giỏ hàng thì cộng thêm số lượng
                    cart[index].Quantity += quantity;
                }
                else
                {
                    //nếu không tồn tại thì thêm sản phẩm vào giỏ hàng
                    cart.Add(new CartModel { Product = qllk.Products.Find(id), Quantity = quantity });
                    //Tính lại số sản phẩm trong giỏ hàng
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });      
        }

        private int isExist(int id)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.ProductID.Equals(id))
                    return i;
            return -1;
        }

        //xóa sản phẩm khỏi giỏ hàng theo id
        public ActionResult Remove(int id)
        {
            List<CartModel> li = (List<CartModel>)Session["cart"];
            li.RemoveAll(x => x.Product.ProductID == id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        //xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveAll()
        {
            List<CartModel> cartItems = (List<CartModel>)Session["cart"];
            cartItems.Clear();
            Session["cart"] = cartItems;
            Session["count"] = 0;
            return Json(new { Message = "Đã xóa tất cả sản phẩm trong giỏ hàng." }, JsonRequestBehavior.AllowGet);
        }

    }
}