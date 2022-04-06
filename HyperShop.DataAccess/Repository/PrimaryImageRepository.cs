
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
    public class PrimaryImageRepository : Repository<PrimaryImage>, IPrimaryImageRepository
    {
        private readonly ApplicationDbContext _db;

        public PrimaryImageRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<PrimaryImage> GetAllByProductId(int productId, string? includeProperties=null)
        {
            IEnumerable<PrimaryImage> primaryImageList = new List<PrimaryImage>();

            if (includeProperties != null)
            {
                foreach(var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    primaryImageList = _db.PrimaryImages.Include(prop);
                }
            }
            else
            {
                primaryImageList = _db.PrimaryImages;
            }

            primaryImageList = primaryImageList.Where(x => x.Product_Id == productId).ToList();
            return primaryImageList;
        }

        public void Update(PrimaryImage obj)
        {
            //dbSet.Update(category);
            _db.PrimaryImages.Update(obj);
        }
    }
}
