
using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderStatusRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(OrderStatus obj)
        {
            //dbSet.Update(category);
            _db.OrderStatuses.Update(obj);
        }
    }
}
