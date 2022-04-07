using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Utility.Class
{
    public class AddToCartObj
    {
        public int productId { get; set; }
        public int colorId { get; set; }
        public List<float> sizeList { get; set; }
    }
}
