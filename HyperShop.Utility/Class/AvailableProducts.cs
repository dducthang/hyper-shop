using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.Utility.Class
{
    public class AvailableProducts
    {
        public int ItemPerPage { get; set; }
        public int PageNumber { get; set; }
        public string Search { get; set; }
        public List<int> Brand { get; set; }
        public List<int> Color { get; set; }
        public List<string> Gender { get; set; }
        public List<string> Height { get; set; }
    }
}
