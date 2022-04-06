using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class CustomerProductListVM
    {
        public Dictionary<Product, int> Products { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Color> Colors { get; set; }

    }
}
