using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string VendorId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? SpecialInstructions { get; set; }

        public int? EstimatedReadyTime { get; set; } // in minutes

        public DateTime? ReadyTime { get; set; }

        public string? PaymentId { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public ApplicationUser Vendor { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
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
