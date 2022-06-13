using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class EditOrderVM
    {
        public Order Order { get; set; }
        public IEnumerable<SelectListItem> OrderStatus { get; set; } 
    }
}
