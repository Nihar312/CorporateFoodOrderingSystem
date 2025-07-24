using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Services;

namespace FoodOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var menuItems = await _menuService.GetAllMenuItemsAsync();
            return Ok(menuItems);
        }

        [HttpGet("canteen/{canteenId}")]
        public async Task<IActionResult> GetMenuByCanteen(int canteenId)
        {
            var menuItems = await _menuService.GetMenuByCanteenAsync(canteenId);
            return Ok(menuItems);
        }

        [HttpGet("canteen/{canteenId}/category/{category}")]
        public async Task<IActionResult> GetMenuByCanteenAndCategory(int canteenId, int category)
        {
            var menuItems = await _menuService.GetMenuByCanteenAndCategoryAsync(canteenId, category);
            return Ok(menuItems);
        }

        [HttpPost]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemDto menuItemDto)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return Unauthorized();

            var menuItem = await _menuService.CreateMenuItemAsync(vendorId, menuItemDto);
            return Ok(menuItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] CreateMenuItemDto menuItemDto)
        {
            try
            {
                var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(vendorId))
                    return Unauthorized();

                var menuItem = await _menuService.UpdateMenuItemAsync(id, vendorId, menuItemDto);
                return Ok(menuItem);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return Unauthorized();

            var result = await _menuService.DeleteMenuItemAsync(id, vendorId);
            if (result)
                return Ok();

            return NotFound();
        }
    }
}