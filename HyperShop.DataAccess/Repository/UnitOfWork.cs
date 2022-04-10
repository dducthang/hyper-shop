using HyperShop.DataAccess.Data;
using HyperShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category= new CategoryRepository(_db);
            Brand= new BrandRepository(_db);
            Product= new ProductRepository(_db);
            Size= new SizeRepository(_db);
            Color= new ColorRepository(_db);
            ProductVariation = new ProductVariationRepository(_db);
            PrimaryImage = new PrimaryImageRepository(_db);
            Image = new ImageRepository(_db);
            Cart = new CartRepository(_db);
            CartDetail = new CartDetailRepository(_db);
            Order = new OrderRepository(_db);
            OrderStatus = new OrderStatusRepository(_db);
            CityShipCost = new CityShipCostRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public ISizeRepository Size { get; private set; }
        public IColorRepository Color{ get; private set; }
        public IProductVariationRepository ProductVariation { get; private set; }
        public IPrimaryImageRepository PrimaryImage { get; private set; }
        public IImageRepository Image { get; private set; }
        public ICartRepository Cart { get; private set; }
        public ICartDetailRepository CartDetail { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IOrderStatusRepository OrderStatus { get; private set; }
        public ICityShipCostRepository CityShipCost { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderDetailRepository OrderDetail { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
