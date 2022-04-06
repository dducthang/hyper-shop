
using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository
{
    public class ProductVariationRepository : Repository<ProductVariation>, IProductVariationRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductVariationRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(ProductVariation obj)
        {
            //dbSet.Update(category);
            _db.ProductVariations.Update(obj);
        }

        public IEnumerable<ProductVariation> GetAllByProductId(int productId, string? includeProperties=null)
        {
            IQueryable<ProductVariation> items = _db.ProductVariations.Where(x => x.Product_Id == productId);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    items = items.Include(prop);
                }
            }
            return items.ToList();
        }

        public IEnumerable<ProductVariation> GetAllByProdIdAndColor(int productId, int colorId, string? includeProperties = null)
        {
            IQueryable<ProductVariation> items = _db.ProductVariations.Where(x => x.Product_Id == productId && x.Color_Id == colorId);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    items = items.Include(prop);
                }
            }
            return items.ToList();
        }
    }
}
