using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [ForeignKey("ProductVariation")]
        public int ProductVariation_Id { get; set; }
        public ProductVariation ProductVariation { get; set; }

        [ForeignKey("Order")]
        public int Order_Id { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }
    }
}
