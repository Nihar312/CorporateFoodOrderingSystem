using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Building
            builder.Entity<Building>(entity =>
            {
                entity.HasMany(e => e.Canteens)
                    .WithOne(e => e.Building)
                    .HasForeignKey(e => e.BuildingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Canteen
            builder.Entity<Canteen>(entity =>
            {
                entity.HasOne(e => e.Building)
                    .WithMany(e => e.Canteens)
                    .HasForeignKey(e => e.BuildingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.MenuItems)
                    .WithOne(e => e.Canteen)
                    .HasForeignKey(e => e.CanteenId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure MenuItem
            builder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Canteen)
                    .WithMany(e => e.MenuItems)
                    .HasForeignKey(e => e.CanteenId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Vendor)
                    .WithMany(e => e.MenuItems)
                    .HasForeignKey(e => e.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Order
            builder.Entity<Order>(entity =>
            {
                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Vendor)
                    .WithMany()
                    .HasForeignKey(e => e.VendorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure OrderItem
            builder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Order)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MenuItem)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}