using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.API.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        public MenuCategory Category { get; set; } = MenuCategory.Lunch;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int CanteenId { get; set; }
        public string VendorId { get; set; } = string.Empty;

        // Navigation properties
        public Canteen Canteen { get; set; } = null!;
        public ApplicationUser Vendor { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum MenuCategory
    {
        Breakfast = 1,
        Snacks = 2,
        Lunch = 3
    }
}