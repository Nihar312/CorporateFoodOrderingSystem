namespace FoodOrderingUI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Category { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public int CanteenId { get; set; }
        public string CanteenName { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FormattedPrice => $"₹{Price:F2}";
    }

    public enum MenuCategory
    {
        Breakfast = 1,
        Snacks = 2,
        Lunch = 3
    }
}
