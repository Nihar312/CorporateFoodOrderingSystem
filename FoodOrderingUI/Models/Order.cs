namespace FoodOrderingUI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string VendorId { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? SpecialInstructions { get; set; }
        public int? EstimatedReadyTime { get; set; }
        public DateTime? ReadyTime { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderItem> Items { get; set; } = new();

        public string FormattedTotal => $"₹{TotalAmount:F2}";
        public string FormattedDate => OrderDate.ToString("dd MMM yyyy HH:mm");
        public string StatusText => Status.ToString();
        public Color StatusColor => Status switch
        {
            OrderStatus.Pending => Colors.Orange,
            OrderStatus.Accepted => Colors.Blue,
            OrderStatus.Preparing => Colors.Purple,
            OrderStatus.Ready => Colors.Green,
            OrderStatus.Completed => Colors.Gray,
            OrderStatus.Cancelled => Colors.Red,
            _ => Colors.Gray
        };
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public string FormattedPrice => $"₹{Price:F2}";
        public string FormattedTotal => $"₹{Price * Quantity:F2}";
    }

    public enum OrderStatus
    {
        Pending = 1,
        Accepted = 2,
        Preparing = 3,
        Ready = 4,
        Completed = 5,
        Cancelled = 6
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4
    }
}
