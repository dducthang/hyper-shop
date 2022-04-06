﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Display(Name="Street Address")]
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }

        [Display(Name="Postal Code")]
        public string? PostalCode { get; set; }
    }
}
