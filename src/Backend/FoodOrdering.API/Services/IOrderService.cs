using FoodOrdering.API.DTOs;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(string userId, CreateOrderDto orderDto);
        Task<OrderResponseDto> GetOrderByIdAsync(int orderId);
        Task<List<OrderResponseDto>> GetOrdersByUserIdAsync(string userId);
        Task<List<OrderResponseDto>> GetOrdersByVendorIdAsync(string vendorId);
        Task<OrderResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto statusDto);
        Task<bool> CancelOrderAsync(int orderId, string userId);
    }
}