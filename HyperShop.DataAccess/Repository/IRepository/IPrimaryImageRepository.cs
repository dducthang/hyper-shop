using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface IPrimaryImageRepository : IRepository<PrimaryImage>
    {
        void Update(PrimaryImage obj);
        IEnumerable<PrimaryImage> GetAllByProductId(int productId, string? includeProperties=null);
    }
}
