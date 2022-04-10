
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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Order obj)
        {
            //dbSet.Update(category);
            _db.Orders.Update(obj);
        }

        public IEnumerable<Order> GetAllByUserId(string userId, string? includeProperties = null)
        {
            IQueryable<Order> items = _db.Orders.Where(x => x.User_Id == userId);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    items = items.Include(prop);
                }
            }
            return items.ToList();
        }

        public Order GetLastestById()
        {
            var order = _db.Orders.OrderByDescending(o => o.Id).ToList();
            return order[0];
        }

    }
}
