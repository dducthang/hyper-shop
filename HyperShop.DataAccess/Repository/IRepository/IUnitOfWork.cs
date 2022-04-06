using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public IBrandRepository Brand{ get; }
        public IProductRepository Product{ get; }
        public ISizeRepository Size{ get; }
        public IColorRepository Color { get; }
        public IProductVariationRepository ProductVariation { get; }
        public IPrimaryImageRepository PrimaryImage { get; }
        public IImageRepository Image { get; }
        public ICartRepository Cart { get; }
        public ICartDetailRepository CartDetail { get; }
        public void Save();
    }
}
