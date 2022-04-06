using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Image
    {
        public int Id { get; set; }
        
        /*[ForeignKey("ProductVariation")]
        public int ProductVariation_Id { get; set; }
        public ProductVariation ProductVariation { get; set; }*/

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Color")]
        public int Color_Id { get; set; }
        public Color Color { get; set; }

        [Required]
        public string ImageUrl{ get; set; }
    }
}
