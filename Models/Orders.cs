using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Orders
    {
        public int id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<OrderLines> OrderLines { get; set; }
    }
}