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
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }


        [ForeignKey("OrderStatus")]
        public int Status_Id { get; set; }
        [ValidateNever]
        public OrderStatus OrderStatus { get; set; }


        [ForeignKey("CityShipCost")]
        [Display(Name ="City")]
        public int CityShipCost_Id { get; set; }

        [ValidateNever]
        public CityShipCost CityShipCost { get; set; }

        public string? Receiver { get; set; }
        public string? Email { get; set; }

        [Display(Name="Phone Number")]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }

        [Display(Name ="Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name ="Total Cost")]
        public double TotalCost { get; set; }
    }
}
