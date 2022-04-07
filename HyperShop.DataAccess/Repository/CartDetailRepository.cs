
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
    public class CartDetailRepository : Repository<CartDetail>, ICartDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public CartDetailRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(CartDetail obj)
        {
            //dbSet.Update(category);
            _db.CartDetails.Update(obj);
        }

        public IEnumerable<CartDetail> GetAllByCartId(int cartId, string? includeProperties = null)
        {
            IQueryable<CartDetail> items = _db.CartDetails.Where(x => x.Cart_Id == cartId);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (prop == "ProductVariation")
                    {
                        var prop2 = "ProductVariation.Size";
                        var prop3 = "ProductVariation.Color";
                        var prop4 = "ProductVariation.Product";
                        items = items.Include(prop2).Include(prop3).Include(prop4);
                    }
                    else
                    {
                        items = items.Include(prop);
                    }
                }
            }
            return items.ToList();
        }

        public IEnumerable<CartDetail> GetAllByProductVariationId(int productVariationId, string? includeProperties = null)
        {
            IQueryable<CartDetail> items = _db.CartDetails.Where(x => x.ProductVariation_Id == productVariationId);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (prop == "ProductVariation")
                    {
                        var prop2 = "ProductVariation.Size";
                        var prop3 = "ProductVariation.Color";
                        var prop4 = "ProductVariation.Product";
                        items = items.Include(prop2).Include(prop3).Include(prop4);
                    }
                    else
                    {
                        items = items.Include(prop);
                    }
                }
            }
            return items.ToList();
        }
    }
}
