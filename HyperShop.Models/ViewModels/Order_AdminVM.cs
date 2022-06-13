using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class Order_AdminVM
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<SelectListItem> OrderStatus{ get; set; }
    }
}
