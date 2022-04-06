using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class VariationEditVM
    {
        [ValidateNever]
        public List<ProductVariation> ProductVariations { get; set; }

        /*[ValidateNever]
        public IEnumerable<SelectListItem> ColorList { get; set; }*/

        [ValidateNever]
        public List<Pair> SizeList { get; set; }

        [ValidateNever]
        public PrimaryImage PrimaryImage { get; set; }

        [ValidateNever]
        public List<IFormFile> ImageList { get; set; }
    }
}
