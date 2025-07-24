using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FoodOrdering.API.Data;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BuildingService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BuildingResponseDto>> GetAllBuildingsAsync()
        {
            var buildings = await _context.Buildings
                .Include(b => b.Canteens)
                .Where(b => b.IsActive)
                .ToListAsync();

            return _mapper.Map<List<BuildingResponseDto>>(buildings);
        }

        public async Task<BuildingResponseDto> GetBuildingByIdAsync(int id)
        {
            var building = await _context.Buildings
                .Include(b => b.Canteens)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

            if (building == null)
                throw new ArgumentException("Building not found");

            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<BuildingResponseDto> CreateBuildingAsync(CreateBuildingDto buildingDto)
        {
            var building = _mapper.Map<Building>(buildingDto);

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<BuildingResponseDto> UpdateBuildingAsync(int id, CreateBuildingDto buildingDto)
        {
            var building = await _context.Buildings.FindAsync(id);

            if (building == null)
                throw new ArgumentException("Building not found");

            _mapper.Map(buildingDto, building);
            await _context.SaveChangesAsync();

            return _mapper.Map<BuildingResponseDto>(building);
        }

        public async Task<bool> DeleteBuildingAsync(int id)
        {
            var building = await _context.Buildings.FindAsync(id);

            if (building == null)
                return false;

            building.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}