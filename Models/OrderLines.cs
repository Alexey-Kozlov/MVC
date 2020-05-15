using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class OrderLines
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Quantity { get; set; }
        public Orders Orders { get; set; }
        public int ProductId { get; set; }
        public Products Products { get; set; }
    }
}