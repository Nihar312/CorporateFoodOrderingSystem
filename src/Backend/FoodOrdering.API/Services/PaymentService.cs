using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace FoodOrdering.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly RazorpayClient _razorpayClient;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            var keyId = _configuration["Razorpay:KeyId"];
            var keySecret = _configuration["Razorpay:KeySecret"];
            _razorpayClient = new RazorpayClient(keyId, keySecret);
        }

        public async Task<PaymentOrderResponse> CreateOrderAsync(decimal amount, string currency = "INR")
        {
            var options = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) }, // Amount in paise
                { "currency", currency },
                { "receipt", Guid.NewGuid().ToString() }
            };

            var order = _razorpayClient.Order.Create(options);

            return new PaymentOrderResponse
            {
                Id = order["id"].ToString(),
                Amount = amount,
                Currency = currency,
                Status = order["status"].ToString()
            };
        }

        public async Task<bool> VerifyPaymentAsync(string orderId, string paymentId, string signature)
        {
            try
            {
                var keySecret = _configuration["Razorpay:KeySecret"];
                var payload = orderId + "|" + paymentId;

                var secretBytes = Encoding.UTF8.GetBytes(keySecret);
                var payloadBytes = Encoding.UTF8.GetBytes(payload);

                using var hmac = new HMACSHA256(secretBytes);
                var computedHash = hmac.ComputeHash(payloadBytes);
                var computedSignature = Convert.ToHexString(computedHash).ToLower();

                return computedSignature == signature.ToLower();
            }
            catch
            {
                return false;
            }
        }
    }
}