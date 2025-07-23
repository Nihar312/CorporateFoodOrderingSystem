using Microsoft.AspNetCore.Identity;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create roles
            string[] roles = { "Admin", "Vendor", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user
            if (await userManager.FindByEmailAsync("admin@foodordering.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@foodordering.com",
                    Email = "admin@foodordering.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    UserType = UserType.Admin,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create sample vendor
            if (await userManager.FindByEmailAsync("vendor@foodordering.com") == null)
            {
                var vendorUser = new ApplicationUser
                {
                    UserName = "vendor@foodordering.com",
                    Email = "vendor@foodordering.com",
                    FirstName = "Sample",
                    LastName = "Vendor",
                    CompanyName = "Delicious Foods",
                    UserType = UserType.Vendor,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(vendorUser, "Vendor123!");
                await userManager.AddToRoleAsync(vendorUser, "Vendor");

                // Add sample menu items
                if (!context.MenuItoms.Any())
                {
                    var MenuItoms = new List<MenuItom>
                    {
                        new MenuItom
                        {
                            Name = "Margherita Pizza",
                            Description = "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil",
                            Price = 299.00m,
                            Category = "Pizza",
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        new MenuItom
                        {
                            Name = "Chicken Biryani",
                            Description = "Aromatic basmati rice cooked with tender chicken and traditional spices",
                            Price = 249.00m,
                            Category = "Main Course",
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        new MenuItom
                        {
                            Name = "Caesar Salad",
                            Description = "Fresh romaine lettuce with caesar dressing, croutons, and parmesan cheese",
                            Price = 179.00m,
                            Category = "Salads",
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        }
                    };

                    context.MenuItoms.AddRange(MenuItoms);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}