using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name{ get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 3000)]
        public double Price { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Height")]
        public string ShoesHeight { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Closure Type")]
        public string ClosureType { get; set; }

        public int ViewCount { get; set; }

        [Required]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

        [Required]
        [ValidateNever]
        public string MainImage { get; set; }       

        [ForeignKey("Brand")]
        [Display(Name="Brand")]
        public int Brand_Id { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }

        [ForeignKey("Category")]
        [Display(Name="Category")]
        public int Category_Id { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        List<ProductVariation> ProductVariations { get; set; }
    }
}
