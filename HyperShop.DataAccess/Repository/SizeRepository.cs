
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
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly ApplicationDbContext _db;

        public SizeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Size obj)
        {
            //dbSet.Update(category);
            _db.Sizes.Update(obj);
        }
    }
}
