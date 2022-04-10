
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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser obj)
        {
            //dbSet.Update(category);
            _db.ApplicationUsers.Update(obj);
        }
    }
}
