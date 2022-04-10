using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models.ViewModels
{
    public class SingleOrderVM
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails{ get; set; }
        public List<PrimaryImage> PrimaryImages{ get; set; }

    }
}
