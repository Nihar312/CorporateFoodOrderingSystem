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

                // Add sample buildings
                if (!context.Buildings.Any())
                {
                    var buildings = new List<Building>
                    {
                        new Building
                        {
                            Name = "Tech Tower",
                            Description = "Main technology building with modern facilities",
                            Location = "North Campus",
                            ImageUrl = "https://images.pexels.com/photos/325229/pexels-photo-325229.jpeg"
                        },
                        new Building
                        {
                            Name = "Business Center",
                            Description = "Corporate offices and meeting facilities",
                            Location = "South Campus",
                            ImageUrl = "https://images.pexels.com/photos/2883049/pexels-photo-2883049.jpeg"
                        },
                        new Building
                        {
                            Name = "Innovation Hub",
                            Description = "Research and development center",
                            Location = "East Campus",
                            ImageUrl = "https://images.pexels.com/photos/1170412/pexels-photo-1170412.jpeg"
                        },
                        new Building
                        {
                            Name = "Executive Plaza",
                            Description = "Executive offices and conference facilities",
                            Location = "West Campus",
                            ImageUrl = "https://images.pexels.com/photos/2883049/pexels-photo-2883049.jpeg"
                        }
                    };

                    context.Buildings.AddRange(buildings);
                    await context.SaveChangesAsync();

                    // Add sample canteens
                    var canteens = new List<Canteen>
                    {
                        new Canteen
                        {
                            Name = "Tech Café",
                            Description = "Modern café with quick bites and beverages",
                            Location = "Ground Floor",
                            OpeningTime = new TimeSpan(7, 0, 0),
                            ClosingTime = new TimeSpan(18, 0, 0),
                            BuildingId = buildings[0].Id,
                            ImageUrl = "https://images.pexels.com/photos/1307698/pexels-photo-1307698.jpeg"
                        },
                        new Canteen
                        {
                            Name = "Business Bistro",
                            Description = "Full-service restaurant with diverse menu",
                            Location = "2nd Floor",
                            OpeningTime = new TimeSpan(8, 0, 0),
                            ClosingTime = new TimeSpan(20, 0, 0),
                            BuildingId = buildings[1].Id,
                            ImageUrl = "https://images.pexels.com/photos/1581384/pexels-photo-1581384.jpeg"
                        },
                        new Canteen
                        {
                            Name = "Innovation Kitchen",
                            Description = "Healthy and organic food options",
                            Location = "1st Floor",
                            OpeningTime = new TimeSpan(7, 30, 0),
                            ClosingTime = new TimeSpan(17, 30, 0),
                            BuildingId = buildings[2].Id,
                            ImageUrl = "https://images.pexels.com/photos/1307698/pexels-photo-1307698.jpeg"
                        }
                    };

                    context.Canteens.AddRange(canteens);
                    await context.SaveChangesAsync();

                    // Add sample menu items
                    var menuItems = new List<MenuItem>
                    {
                        // Tech Café - Breakfast
                        new MenuItem
                        {
                            Name = "Continental Breakfast",
                            Description = "Toast, butter, jam, and fresh juice",
                            Price = 120.00m,
                            Category = MenuCategory.Breakfast,
                            CanteenId = canteens[0].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true,
                            ImageUrl = "https://images.pexels.com/photos/1099680/pexels-photo-1099680.jpeg"
                        },
                        new MenuItem
                        {
                            Name = "Masala Chai",
                            Description = "Traditional Indian spiced tea",
                            Price = 25.00m,
                            Category = MenuCategory.Breakfast,
                            CanteenId = canteens[0].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        // Tech Café - Snacks
                        new MenuItem
                        {
                            Name = "Samosa",
                            Description = "Crispy pastry with spiced potato filling",
                            Price = 35.00m,
                            Category = MenuCategory.Snacks,
                            CanteenId = canteens[0].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        new MenuItem
                        {
                            Name = "Sandwich",
                            Description = "Grilled vegetable sandwich",
                            Price = 80.00m,
                            Category = MenuCategory.Snacks,
                            CanteenId = canteens[0].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        // Business Bistro - Lunch
                        new MenuItem
                        {
                            Name = "Chicken Biryani",
                            Description = "Aromatic basmati rice with tender chicken",
                            Price = 180.00m,
                            Category = MenuCategory.Lunch,
                            CanteenId = canteens[1].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true,
                            ImageUrl = "https://images.pexels.com/photos/1624487/pexels-photo-1624487.jpeg"
                        },
                        new MenuItem
                        {
                            Name = "Dal Rice Combo",
                            Description = "Yellow dal with steamed rice and pickle",
                            Price = 120.00m,
                            Category = MenuCategory.Lunch,
                            CanteenId = canteens[1].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        // Innovation Kitchen - Healthy Options
                        new MenuItem
                        {
                            Name = "Quinoa Salad",
                            Description = "Fresh quinoa with mixed vegetables",
                            Price = 150.00m,
                            Category = MenuCategory.Lunch,
                            CanteenId = canteens[2].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        },
                        new MenuItem
                        {
                            Name = "Green Smoothie",
                            Description = "Spinach, apple, and banana smoothie",
                            Price = 90.00m,
                            Category = MenuCategory.Snacks,
                            CanteenId = canteens[2].Id,
                            VendorId = vendorUser.Id,
                            IsAvailable = true
                        }
                    };

                    context.MenuItems.AddRange(menuItems);
                    await context.SaveChangesAsync();

                    // Add payment methods
                    if (!context.PaymentMethods.Any())
                    {
                        var paymentMethods = new List<PaymentMethod>
                        {
                            new PaymentMethod
                            {
                                Name = "Corporate Wallet",
                                Description = "Company provided digital wallet",
                                Type = PaymentType.Wallet,
                                IconUrl = "wallet-icon.png"
                            },
                            new PaymentMethod
                            {
                                Name = "Credit/Debit Card",
                                Description = "Pay using your card",
                                Type = PaymentType.Card,
                                IconUrl = "card-icon.png"
                            },
                            new PaymentMethod
                            {
                                Name = "UPI Payment",
                                Description = "Pay using UPI apps",
                                Type = PaymentType.UPI,
                                IconUrl = "upi-icon.png"
                            },
                            new PaymentMethod
                            {
                                Name = "Monthly Billing",
                                Description = "Add to monthly salary deduction",
                                Type = PaymentType.MonthlyBilling,
                                IconUrl = "billing-icon.png"
                            }
                        };

                        context.PaymentMethods.AddRange(paymentMethods);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}