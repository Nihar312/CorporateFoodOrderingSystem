using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FoodOrdering.API.Data;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public class CanteenService : ICanteenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CanteenService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CanteenResponseDto>> GetAllCanteensAsync()
        {
            var canteens = await _context.Canteens
                .Include(c => c.Building)
                .Include(c => c.MenuItems)
                .Where(c => c.IsActive)
                .ToListAsync();

            return _mapper.Map<List<CanteenResponseDto>>(canteens);
        }

        public async Task<List<CanteenResponseDto>> GetCanteensByBuildingAsync(int buildingId)
        {
            var canteens = await _context.Canteens
                .Include(c => c.Building)
                .Include(c => c.MenuItems)
                .Where(c => c.BuildingId == buildingId && c.IsActive)
                .ToListAsync();

            return _mapper.Map<List<CanteenResponseDto>>(canteens);
        }

        public async Task<CanteenResponseDto> GetCanteenByIdAsync(int id)
        {
            var canteen = await _context.Canteens
                .Include(c => c.Building)
                .Include(c => c.MenuItems)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (canteen == null)
                throw new ArgumentException("Canteen not found");

            return _mapper.Map<CanteenResponseDto>(canteen);
        }

        public async Task<CanteenResponseDto> CreateCanteenAsync(CreateCanteenDto canteenDto)
        {
            var canteen = _mapper.Map<Canteen>(canteenDto);

            _context.Canteens.Add(canteen);
            await _context.SaveChangesAsync();

            var createdCanteen = await _context.Canteens
                .Include(c => c.Building)
                .FirstOrDefaultAsync(c => c.Id == canteen.Id);

            return _mapper.Map<CanteenResponseDto>(createdCanteen);
        }

        public async Task<CanteenResponseDto> UpdateCanteenAsync(int id, CreateCanteenDto canteenDto)
        {
            var canteen = await _context.Canteens
                .Include(c => c.Building)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (canteen == null)
                throw new ArgumentException("Canteen not found");

            _mapper.Map(canteenDto, canteen);
            await _context.SaveChangesAsync();

            return _mapper.Map<CanteenResponseDto>(canteen);
        }

        public async Task<bool> DeleteCanteenAsync(int id)
        {
            var canteen = await _context.Canteens.FindAsync(id);

            if (canteen == null)
                return false;

            canteen.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}