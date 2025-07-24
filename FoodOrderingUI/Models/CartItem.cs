namespace FoodOrderingUI.Models
{
    public class CartItem
    {
        public MenuItem MenuItem { get; set; } = null!;
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public decimal Total => MenuItem.Price * Quantity;
        public string FormattedTotal => $"₹{Total:F2}";
    }

}
