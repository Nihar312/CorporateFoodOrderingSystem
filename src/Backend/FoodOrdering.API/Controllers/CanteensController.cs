using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Services;

namespace FoodOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CanteensController : ControllerBase
    {
        private readonly ICanteenService _canteenService;

        public CanteensController(ICanteenService canteenService)
        {
            _canteenService = canteenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCanteens()
        {
            var canteens = await _canteenService.GetAllCanteensAsync();
            return Ok(canteens);
        }

        [HttpGet("building/{buildingId}")]
        public async Task<IActionResult> GetCanteensByBuilding(int buildingId)
        {
            var canteens = await _canteenService.GetCanteensByBuildingAsync(buildingId);
            return Ok(canteens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCanteen(int id)
        {
            try
            {
                var canteen = await _canteenService.GetCanteenByIdAsync(id);
                return Ok(canteen);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCanteen([FromBody] CreateCanteenDto canteenDto)
        {
            var canteen = await _canteenService.CreateCanteenAsync(canteenDto);
            return Ok(canteen);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCanteen(int id, [FromBody] CreateCanteenDto canteenDto)
        {
            try
            {
                var canteen = await _canteenService.UpdateCanteenAsync(id, canteenDto);
                return Ok(canteen);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCanteen(int id)
        {
            var result = await _canteenService.DeleteCanteenAsync(id);
            if (result)
                return Ok();

            return NotFound();
        }
    }
}