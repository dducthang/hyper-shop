using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface IProductVariationRepository : IRepository<ProductVariation>
    {
        void Update(ProductVariation obj);
        public IEnumerable<ProductVariation> GetAllByProductId(int productId, string? includeProperties=null);
        public IEnumerable<ProductVariation> GetAllByProdIdAndColor(int productId, int colorId, string? includeProperties = null);
    }
}
