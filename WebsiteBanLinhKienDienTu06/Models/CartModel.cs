using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanLinhKienDienTu06.Context;

namespace WebsiteBanLinhKienDienTu06.Models
{
    public class CartModel
    {
      
        public Product Product { get; set; }
       
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Total { get { return UnitPrice * Quantity; } }

        
    }
}