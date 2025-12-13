using AgroShop.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AgroShop.Web.Data
{
    public class AgroShopContext : DbContext
    {
        public AgroShopContext(DbContextOptions<AgroShopContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optionally configure keys, defaults, and relationships explicitly if needed
            // Example: modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(10,2)");
        }
    }
}
