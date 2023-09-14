using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanLinhKienDienTu06.Models
{
    public class InvoiceModel
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public decimal TotalPrice { get; set; }
    }
}