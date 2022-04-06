
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
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private readonly ApplicationDbContext _db;

        public ColorRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Color obj)
        {
            //dbSet.Update(category);
            _db.Colors.Update(obj);
        }
    }
}
