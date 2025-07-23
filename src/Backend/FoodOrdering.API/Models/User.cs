// Models/User.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType UserType { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? FCMToken { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<MenuItom> MenuItoms { get; set; } = new List<MenuItom>();
    }

    public enum UserType
    {
        User = 1,
        Vendor = 2,
        Admin = 3
    }
}
