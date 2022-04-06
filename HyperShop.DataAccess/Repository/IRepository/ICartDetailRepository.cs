using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface ICartDetailRepository : IRepository<CartDetail>
    {
        void Update(CartDetail obj);
        IEnumerable<CartDetail> GetAllByCartId(int cartId, string? includeProperties = null);
    }
}
