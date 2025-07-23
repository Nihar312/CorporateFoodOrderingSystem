namespace FoodOrdering.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentOrderResponse> CreateOrderAsync(decimal amount, string currency = "INR");
        Task<bool> VerifyPaymentAsync(string orderId, string paymentId, string signature);
    }

    public class PaymentOrderResponse
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}