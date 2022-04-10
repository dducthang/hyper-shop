using HyperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface ICityShipCostRepository : IRepository<CityShipCost>
    {
        void Update(CityShipCost obj);
    }
}
