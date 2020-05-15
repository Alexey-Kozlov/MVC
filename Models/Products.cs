using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Products
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Необходимо указать наименование")]
        [Display(Name ="Наименование продукта")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Необходимо указать цену")]
        [Display(Name ="Цена продукта")]
        public decimal Price { get; set; }
    }
}