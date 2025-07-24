using System.ComponentModel.DataAnnotations;

namespace FoodOrdering.API.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        public PaymentType Type { get; set; }

        public bool IsActive { get; set; } = true;

        public string? IconUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum PaymentType
    {
        Wallet = 1,
        Card = 2,
        UPI = 3,
        MonthlyBilling = 4
    }
}