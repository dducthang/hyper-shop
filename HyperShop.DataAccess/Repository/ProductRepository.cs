
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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            //dbSet.Update(category);
            //_db.Products.Update(obj);

            var objFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Gender = obj.Gender;
                objFromDb.ShoesHeight = obj.ShoesHeight;
                objFromDb.ClosureType = obj.ClosureType;
                objFromDb.ViewCount = obj.ViewCount;
                objFromDb.PublishedDate = obj.PublishedDate;
                objFromDb.Brand_Id = obj.Brand_Id;
                objFromDb.Category_Id = obj.Category_Id;
                if (obj.MainImage != null)
                {
                    objFromDb.MainImage = obj.MainImage; 
                }
            }
        }
    }
}
