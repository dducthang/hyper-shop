using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order obj);
        IEnumerable<Order> GetAllByUserId(string userId, string? includeProperties=null);
        Order GetLastestById();
    }
}
