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
        public async Task<IActionResult> GetAllMenuItoms()
        {
            var MenuItoms = await _menuService.GetAllMenuItomsAsync();
            return Ok(MenuItoms);
        }

        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetMenuByVendor(string vendorId)
        {
            var MenuItoms = await _menuService.GetMenuByVendorAsync(vendorId);
            return Ok(MenuItoms);
        }

        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> CreateMenuItom([FromBody] CreateMenuItomDto MenuItomDto)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return Unauthorized();

            var MenuItom = await _menuService.CreateMenuItomAsync(vendorId, MenuItomDto);
            return Ok(MenuItom);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> UpdateMenuItom(int id, [FromBody] CreateMenuItomDto MenuItomDto)
        {
            try
            {
                var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(vendorId))
                    return Unauthorized();

                var MenuItom = await _menuService.UpdateMenuItomAsync(id, vendorId, MenuItomDto);
                return Ok(MenuItom);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Vendor")]
        public async Task<IActionResult> DeleteMenuItom(int id)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
                return Unauthorized();

            var result = await _menuService.DeleteMenuItomAsync(id, vendorId);
            if (result)
                return Ok();

            return NotFound();
        }
    }
}