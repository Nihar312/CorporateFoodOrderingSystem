using FoodOrderingUI.Models;
using MenuItem = FoodOrderingUI.Models.MenuItem;

namespace FoodOrderingUI.Services
{
    public interface IApiService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RegisterAsync(string email, string password, string firstName, string lastName, string? companyName = null);
        Task<List<MenuItem>> GetMenuItemsAsync();
        Task<List<MenuItem>> GetMenuByVendorAsync(string vendorId);
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<List<Order>> GetMyOrdersAsync();
        Task<List<Order>> GetVendorOrdersAsync();
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status, int? estimatedReadyTime = null);
        Task<bool> CancelOrderAsync(int orderId);
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateOrderRequest
    {
        public string VendorId { get; set; } = string.Empty;
        public List<CreateOrderItem> Items { get; set; } = new();
        public string? SpecialInstructions { get; set; }
    }

    public class CreateOrderItem
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }

}
