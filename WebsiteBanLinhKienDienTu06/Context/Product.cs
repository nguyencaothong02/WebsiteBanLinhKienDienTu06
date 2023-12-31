//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebsiteBanLinhKienDienTu06.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Ratings = new HashSet<Rating>();
        }
    
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<bool> Discoutinued { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
