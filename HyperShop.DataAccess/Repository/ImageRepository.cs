
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
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly ApplicationDbContext _db;

        public ImageRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<Image> GetAllByProdAndColorId(int productId, int colorId, string? includeProperties=null)
        {
            IEnumerable<Image> imageList = new List<Image>();

            if (includeProperties != null)
            {
                foreach(var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    imageList = _db.Images.Include(prop);
                }
            }
            else
            {
                imageList = _db.Images.ToList();
            }

            imageList = imageList.Where(x => x.Product_Id == productId && x.Color_Id==colorId).ToList();
            return imageList;
        }

        public IEnumerable<Image> GetAllByProductId(int productId, string? includeProperties = null)
        {
            IEnumerable<Image> imageList = new List<Image>();
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    imageList = _db.Images.Include(prop);
                }
            }
            else
            {
                imageList = _db.Images;
            }

            imageList = imageList.Where(x => x.Product_Id == productId).ToList();
            return imageList;
        }

        public void Update(Image obj)
        {
            //dbSet.Update(category);
            _db.Images.Update(obj);
        }
    }
}
