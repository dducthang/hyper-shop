using HyperShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HyperShop.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Color> Colors{ get; set; }
        public DbSet<Size> Sizes{ get; set; }
        public DbSet<ProductVariation> ProductVariations{ get; set; }
        public DbSet<Image> Images{ get; set; }
        public DbSet<PrimaryImage> PrimaryImages{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Cart> Carts{ get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
    }
}