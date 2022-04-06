using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class ProductVariation
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("Color")]
        public int Color_Id { get; set; }
        public Color Color { get; set; }
        
        [ForeignKey("Size")]
        public int Size_Id { get; set; }
        public Size Size { get; set; }

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }
    }
}
