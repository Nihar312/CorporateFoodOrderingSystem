using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.API.Models
{
    public class Canteen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string Location { get; set; } = string.Empty;

        public TimeSpan OpeningTime { get; set; }

        public TimeSpan ClosingTime { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int BuildingId { get; set; }

        // Navigation properties
        public Building Building { get; set; } = null!;
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}