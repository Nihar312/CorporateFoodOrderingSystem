using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Services
{
    public interface IMenuService
    {
        Task<List<MenuItomResponseDto>> GetAllMenuItomsAsync();
        Task<List<MenuItomResponseDto>> GetMenuByVendorAsync(string vendorId);
        Task<MenuItomResponseDto> CreateMenuItomAsync(string vendorId, CreateMenuItomDto MenuItomDto);
        Task<MenuItomResponseDto> UpdateMenuItomAsync(int id, string vendorId, CreateMenuItomDto MenuItomDto);
        Task<bool> DeleteMenuItomAsync(int id, string vendorId);
    }
}