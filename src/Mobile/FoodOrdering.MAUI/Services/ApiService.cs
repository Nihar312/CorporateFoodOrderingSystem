using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FoodOrdering.MAUI.Models;

namespace FoodOrdering.MAUI.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private const string BaseUrl = "https://your-api-url.com/api/";

        public ApiService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        private async Task SetAuthenticationHeader()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var request = new { Email = email, Password = password };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (authResponse != null)
            {
                await _tokenService.SetTokenAsync(authResponse.Token);
            }

            return authResponse!;
        }

        public async Task<AuthResponse> RegisterAsync(string email, string password, string firstName, string lastName, string? companyName = null)
        {
            var request = new { Email = email, Password = password, FirstName = firstName, LastName = lastName, CompanyName = companyName };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("auth/register", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (authResponse != null)
            {
                await _tokenService.SetTokenAsync(authResponse.Token);
            }

            return authResponse!;
        }

        public async Task<List<MenuItom>> GetMenuItomsAsync()
        {
            await SetAuthenticationHeader();

            var response = await _httpClient.GetAsync("menu");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var MenuItoms = JsonSerializer.Deserialize<List<MenuItom>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return MenuItoms ?? new List<MenuItom>();
        }

        public async Task<List<MenuItom>> GetMenuByVendorAsync(string vendorId)
        {
            await SetAuthenticationHeader();

            var response = await _httpClient.GetAsync($"menu/vendor/{vendorId}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var MenuItoms = JsonSerializer.Deserialize<List<MenuItom>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return MenuItoms ?? new List<MenuItom>();
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            await SetAuthenticationHeader();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("orders", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<Order>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return order!;
        }

        public async Task<List<Order>> GetMyOrdersAsync()
        {
            await SetAuthenticationHeader();

            var response = await _httpClient.GetAsync("orders/my-orders");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return orders ?? new List<Order>();
        }

        public async Task<List<Order>> GetVendorOrdersAsync()
        {
            await SetAuthenticationHeader();

            var response = await _httpClient.GetAsync("orders/vendor-orders");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return orders ?? new List<Order>();
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status, int? estimatedReadyTime = null)
        {
            await SetAuthenticationHeader();

            var request = new { Status = status, EstimatedReadyTime = estimatedReadyTime };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"orders/{orderId}/status", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var order = JsonSerializer.Deserialize<Order>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return order!;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            await SetAuthenticationHeader();

            var response = await _httpClient.DeleteAsync($"orders/{orderId}");
            return response.IsSuccessStatusCode;
        }
    }
}