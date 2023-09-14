using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanLinhKienDienTu06.Models
{
    public class CommentViewModel
    {
        public int CommentID { get; set; }
        public int ProductID { get; set; }
        public string CustomerID { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}