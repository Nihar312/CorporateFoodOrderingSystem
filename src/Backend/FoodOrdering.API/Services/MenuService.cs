using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FoodOrdering.API.Data;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Models;

namespace FoodOrdering.API.Services
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MenuService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MenuItemResponseDto>> GetAllMenuItemsAsync()
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.Canteen)
                    .ThenInclude(c => c.Building)
                .Include(m => m.Vendor)
                .Where(m => m.IsAvailable)
                .ToListAsync();

            return _mapper.Map<List<MenuItemResponseDto>>(menuItems);
        }

        public async Task<List<MenuItemResponseDto>> GetMenuByCanteenAsync(int canteenId)
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.Canteen)
                    .ThenInclude(c => c.Building)
                .Include(m => m.Vendor)
                .Where(m => m.CanteenId == canteenId && m.IsAvailable)
                .ToListAsync();

            return _mapper.Map<List<MenuItemResponseDto>>(menuItems);
        }

        public async Task<List<MenuItemResponseDto>> GetMenuByCanteenAndCategoryAsync(int canteenId, int category)
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.Canteen)
                    .ThenInclude(c => c.Building)
                .Include(m => m.Vendor)
                .Where(m => m.CanteenId == canteenId && (int)m.Category == category && m.IsAvailable)
                .ToListAsync();

            return _mapper.Map<List<MenuItemResponseDto>>(menuItems);
        }

        public async Task<MenuItemResponseDto> CreateMenuItemAsync(string vendorId, CreateMenuItemDto menuItemDto)
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemDto);
            menuItem.VendorId = vendorId;

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            var createdItem = await _context.MenuItems
                .Include(m => m.Canteen)
                    .ThenInclude(c => c.Building)
                .Include(m => m.Vendor)
                .FirstOrDefaultAsync(m => m.Id == menuItem.Id);

            return _mapper.Map<MenuItemResponseDto>(createdItem);
        }

        public async Task<MenuItemResponseDto> UpdateMenuItemAsync(int id, string vendorId, CreateMenuItemDto menuItemDto)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.Canteen)
                    .ThenInclude(c => c.Building)
                .Include(m => m.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id && m.VendorId == vendorId);

            if (menuItem == null)
                throw new ArgumentException("Menu item not found");

            _mapper.Map(menuItemDto, menuItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<MenuItemResponseDto>(menuItem);
        }

        public async Task<bool> DeleteMenuItemAsync(int id, string vendorId)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id && m.VendorId == vendorId);

            if (menuItem == null)
                return false;

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}