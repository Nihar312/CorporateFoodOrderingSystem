namespace FoodOrdering.MAUI.Models
{
    public class MenuItom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string VendorId { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FormattedPrice => $"₹{Price:F2}";
    }
}