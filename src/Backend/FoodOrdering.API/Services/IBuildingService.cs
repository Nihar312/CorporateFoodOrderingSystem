using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Services
{
    public interface IBuildingService
    {
        Task<List<BuildingResponseDto>> GetAllBuildingsAsync();
        Task<BuildingResponseDto> GetBuildingByIdAsync(int id);
        Task<BuildingResponseDto> CreateBuildingAsync(CreateBuildingDto buildingDto);
        Task<BuildingResponseDto> UpdateBuildingAsync(int id, CreateBuildingDto buildingDto);
        Task<bool> DeleteBuildingAsync(int id);
    }
}