using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodOrdering.API.Services;

namespace FoodOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] CreatePaymentOrderRequest request)
        {
            try
            {
                var paymentOrder = await _paymentService.CreateOrderAsync(request.Amount);
                return Ok(paymentOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            try
            {
                var isValid = await _paymentService.VerifyPaymentAsync(
                    request.OrderId,
                    request.PaymentId,
                    request.Signature);

                if (isValid)
                {
                    return Ok(new { status = "success" });
                }

                return BadRequest(new { status = "failure" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class CreatePaymentOrderRequest
    {
        public decimal Amount { get; set; }
    }

    public class VerifyPaymentRequest
    {
        public string OrderId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}