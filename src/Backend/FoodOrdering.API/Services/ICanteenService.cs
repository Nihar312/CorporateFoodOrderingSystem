using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Services
{
    public interface ICanteenService
    {
        Task<List<CanteenResponseDto>> GetAllCanteensAsync();
        Task<List<CanteenResponseDto>> GetCanteensByBuildingAsync(int buildingId);
        Task<CanteenResponseDto> GetCanteenByIdAsync(int id);
        Task<CanteenResponseDto> CreateCanteenAsync(CreateCanteenDto canteenDto);
        Task<CanteenResponseDto> UpdateCanteenAsync(int id, CreateCanteenDto canteenDto);
        Task<bool> DeleteCanteenAsync(int id);
    }
}