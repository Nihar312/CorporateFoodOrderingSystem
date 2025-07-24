using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Services;

namespace FoodOrdering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingsController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuildings()
        {
            var buildings = await _buildingService.GetAllBuildingsAsync();
            return Ok(buildings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuilding(int id)
        {
            try
            {
                var building = await _buildingService.GetBuildingByIdAsync(id);
                return Ok(building);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBuilding([FromBody] CreateBuildingDto buildingDto)
        {
            var building = await _buildingService.CreateBuildingAsync(buildingDto);
            return Ok(building);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBuilding(int id, [FromBody] CreateBuildingDto buildingDto)
        {
            try
            {
                var building = await _buildingService.UpdateBuildingAsync(id, buildingDto);
                return Ok(building);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            var result = await _buildingService.DeleteBuildingAsync(id);
            if (result)
                return Ok();

            return NotFound();
        }
    }
}