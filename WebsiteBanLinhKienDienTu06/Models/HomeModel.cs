using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Models
{
    public class HomeModel
    {
        public List<Product> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }
        public List<Order> ListOrder { get; set; }
        public List<Customer> ListCustomer { get; set; }

    }
}