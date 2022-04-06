using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class CartDetail
    {
        public int Id { get; set; }

        [ForeignKey("ProductVariation")]
        public int ProductVariation_Id { get; set; }
        public ProductVariation ProductVariation{ get; set; }

        [ForeignKey("Cart")]
        public int Cart_Id { get; set; }
        public Cart Cart { get; set; }

        public int Quantity { get; set; }

    }
}
