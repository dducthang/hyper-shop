using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class CheckoutVM
    {
        public Order Order { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
