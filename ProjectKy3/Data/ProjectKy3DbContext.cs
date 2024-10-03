using Microsoft.EntityFrameworkCore;
using ProjectKy3.Models;

namespace ProjectKy3.Data
{
    public class ProjectKy3DbContext : DbContext
    {
        public ProjectKy3DbContext(DbContextOptions<ProjectKy3DbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Return> Returns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<ProductVariant>().HasKey(pv => pv.VariantId);
            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Product>().HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId);
            modelBuilder.Entity<ProductVariant>().HasOne(pv => pv.Product).WithMany(p => p.ProductVariants).HasForeignKey(pv => pv.ProductId);
            modelBuilder.Entity<ProductVariant>().HasOne(pv => pv.Color).WithMany().HasForeignKey(pv => pv.ColorId);
            modelBuilder.Entity<ProductVariant>().HasOne(pv => pv.Size).WithMany().HasForeignKey(pv => pv.SizeId);
            modelBuilder.Entity<CartItem>().HasOne(ci => ci.User).WithMany(u => u.CartItems).HasForeignKey(ci => ci.UserId);
            modelBuilder.Entity<CartItem>().HasOne(ci => ci.Variant).WithMany().HasForeignKey(ci => ci.VariantId);
            modelBuilder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId);
            modelBuilder.Entity<Review>().HasOne(r => r.Product).WithMany(p => p.Reviews).HasForeignKey(r => r.ProductId);
            modelBuilder.Entity<Favorite>().HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId);
            modelBuilder.Entity<Favorite>().HasOne(f => f.Product).WithMany(p => p.Favorites).HasForeignKey(f => f.ProductId);
            modelBuilder.Entity<Coupon>().HasOne(c => c.Product).WithMany().HasForeignKey(c => c.ProductId);
            modelBuilder.Entity<Shipping>().HasOne(s => s.Order).WithMany().HasForeignKey(s => s.OrderId);
            modelBuilder.Entity<Return>().HasOne(r => r.Order).WithMany().HasForeignKey(r => r.OrderId);
        }
    }
}
