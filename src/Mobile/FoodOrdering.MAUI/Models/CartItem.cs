namespace FoodOrdering.MAUI.Models
{
    public class CartItem
    {
        public MenuItom MenuItom { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public decimal Total => MenuItom.Price * Quantity;
        public string FormattedTotal => $"₹{Total:F2}";
    }
}