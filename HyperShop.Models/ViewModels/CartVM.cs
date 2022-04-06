using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class CartVM
    {
        public List<PrimaryImage> Images { get; set; }
        public List<CartDetail> CartDetails { get; set; }
    }
}
