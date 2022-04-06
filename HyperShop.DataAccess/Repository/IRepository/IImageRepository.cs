using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface IImageRepository : IRepository<Image>
    {
        void Update(Image obj);
        IEnumerable<Image> GetAllByProductId(int productId, string? includeProperties=null);
        IEnumerable<Image> GetAllByProdAndColorId(int productId, int colorId, string? includeProperties=null);
    }
}
