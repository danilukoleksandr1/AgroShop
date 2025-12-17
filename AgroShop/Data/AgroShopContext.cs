using AgroShop.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroShop.Web.Data
{
    public class AgroShopContext : DbContext
    {
        public AgroShopContext(DbContextOptions<AgroShopContext> options)
            : base(options) { }

        // ===== ОСНОВНІ =====
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Review> Reviews => Set<Review>();

        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<ShippingMethod> ShippingMethods => Set<ShippingMethod>();

        // ===== ЗАМОВЛЕННЯ =====
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderContact> OrderContacts => Set<OrderContact>();
        public DbSet<OrderAddress> OrderAddresses => Set<OrderAddress>();
        public DbSet<OrderShipping> OrderShipping => Set<OrderShipping>();
        public DbSet<OrderPayment> OrderPayments => Set<OrderPayment>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== STATUS =====
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany()
                .HasForeignKey(o => o.StatusID);

            // ===== 1 : 1 =====
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Contact)
                .WithOne(c => c.Order)
                .HasForeignKey<OrderContact>(c => c.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithOne(a => a.Order)
                .HasForeignKey<OrderAddress>(a => a.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipping)
                .WithOne(s => s.Order)
                .HasForeignKey<OrderShipping>(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== 1 : M =====
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Details)
                .WithOne(d => d.Order)
                .HasForeignKey(d => d.OrderID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderID);
        }
    }
}
