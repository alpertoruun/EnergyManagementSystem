using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HousesController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public HousesController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HouseDto>> GetById(int id)
        {
            var house = await _houseService.GetByIdAsync(id);
            return house;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<HouseDto>>> GetUserHouses(int userId)
        {
            var houses = await _houseService.GetUserHousesAsync(userId);
            return Ok(houses);
        }

        [HttpPost]
        public async Task<ActionResult<HouseDto>> Create(CreateHouseDto createHouseDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var house = await _houseService.CreateAsync(userId, createHouseDto);
            return CreatedAtAction(nameof(GetById), new { id = house.HouseId }, house);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateHouseDto updateHouseDto)
        {
            await _houseService.UpdateAsync(id, updateHouseDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _houseService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/power-saving")]
        public async Task<IActionResult> TogglePowerSaving(int id, [FromBody] bool enabled)
        {
            await _houseService.TogglePowerSavingModeAsync(id, enabled);
            return NoContent();
        }
    }
}
