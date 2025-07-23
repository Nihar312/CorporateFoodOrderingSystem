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

        public DbSet<MenuItom> MenuItoms { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure MenuItom
            builder.Entity<MenuItom>(entity =>
            {
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Vendor)
                    .WithMany(e => e.MenuItoms)
                    .HasForeignKey(e => e.VendorId)
                    .OnDelete(DeleteBehavior.Cascade);
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

                entity.HasOne(e => e.MenuItom)
                    .WithMany(e => e.OrderItems)
                    .HasForeignKey(e => e.MenuItomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}