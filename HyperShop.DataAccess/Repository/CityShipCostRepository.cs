
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
    public class CityShipCostRepository : Repository<CityShipCost>, ICityShipCostRepository
    {
        private readonly ApplicationDbContext _db;

        public CityShipCostRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(CityShipCost obj)
        {
            //dbSet.Update(category);
            _db.CityShipCosts.Update(obj);
        }
    }
}
