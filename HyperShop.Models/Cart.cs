using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Total { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        public ApplicationUser User { get; set; }
    }
}
