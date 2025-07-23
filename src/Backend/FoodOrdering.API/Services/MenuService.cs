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

        public async Task<List<MenuItomResponseDto>> GetAllMenuItomsAsync()
        {
            var MenuItoms = await _context.MenuItoms
                .Include(m => m.Vendor)
                .Where(m => m.IsAvailable)
                .ToListAsync();

            return _mapper.Map<List<MenuItomResponseDto>>(MenuItoms);
        }

        public async Task<List<MenuItomResponseDto>> GetMenuByVendorAsync(string vendorId)
        {
            var MenuItoms = await _context.MenuItoms
                .Include(m => m.Vendor)
                .Where(m => m.VendorId == vendorId && m.IsAvailable)
                .ToListAsync();

            return _mapper.Map<List<MenuItomResponseDto>>(MenuItoms);
        }

        public async Task<MenuItomResponseDto> CreateMenuItomAsync(string vendorId, CreateMenuItomDto MenuItomDto)
        {
            var MenuItom = _mapper.Map<MenuItom>(MenuItomDto);
            MenuItom.VendorId = vendorId;

            _context.MenuItoms.Add(MenuItom);
            await _context.SaveChangesAsync();

            var createdItem = await _context.MenuItoms
                .Include(m => m.Vendor)
                .FirstOrDefaultAsync(m => m.Id == MenuItom.Id);

            return _mapper.Map<MenuItomResponseDto>(createdItem);
        }

        public async Task<MenuItomResponseDto> UpdateMenuItomAsync(int id, string vendorId, CreateMenuItomDto MenuItomDto)
        {
            var MenuItom = await _context.MenuItoms
                .Include(m => m.Vendor)
                .FirstOrDefaultAsync(m => m.Id == id && m.VendorId == vendorId);

            if (MenuItom == null)
                throw new ArgumentException("Menu item not found");

            _mapper.Map(MenuItomDto, MenuItom);
            await _context.SaveChangesAsync();

            return _mapper.Map<MenuItomResponseDto>(MenuItom);
        }

        public async Task<bool> DeleteMenuItomAsync(int id, string vendorId)
        {
            var MenuItom = await _context.MenuItoms
                .FirstOrDefaultAsync(m => m.Id == id && m.VendorId == vendorId);

            if (MenuItom == null)
                return false;

            _context.MenuItoms.Remove(MenuItom);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}