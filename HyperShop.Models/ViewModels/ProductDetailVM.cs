using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<PrimaryImage> PrimaryImages { get; set; }
        public List<ProductVariation> Variations { get; set; }
    }
}
