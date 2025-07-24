using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Services
{
    public interface IMenuService
    {
        Task<List<MenuItemResponseDto>> GetAllMenuItemsAsync();
        Task<List<MenuItemResponseDto>> GetMenuByCanteenAsync(int canteenId);
        Task<List<MenuItemResponseDto>> GetMenuByCanteenAndCategoryAsync(int canteenId, int category);
        Task<MenuItemResponseDto> CreateMenuItemAsync(string vendorId, CreateMenuItemDto menuItemDto);
        Task<MenuItemResponseDto> UpdateMenuItemAsync(int id, string vendorId, CreateMenuItemDto menuItemDto);
        Task<bool> DeleteMenuItemAsync(int id, string vendorId);
    }
}