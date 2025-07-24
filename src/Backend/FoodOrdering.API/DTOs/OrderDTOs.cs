using FoodOrdering.API.Models;

namespace FoodOrdering.API.DTOs
{
    public class CreateOrderDto
    {
        public string VendorId { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
        public string? SpecialInstructions { get; set; }
    }

    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }

    public class OrderResponseDto
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
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
        public int? EstimatedReadyTime { get; set; }
    }
}