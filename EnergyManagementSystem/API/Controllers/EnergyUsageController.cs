using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.DTOs.EnergyUsage;
using EnergyManagementSystem.Core.Interfaces;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyUsageController : ControllerBase
    {
        private readonly IEnergyUsageService _energyUsageService;

        public EnergyUsageController(IEnergyUsageService energyUsageService)
        {
            _energyUsageService = energyUsageService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnergyUsageDto>> GetById(int id)
        {
            return await _energyUsageService.GetByIdAsync(id);
        }

        [HttpGet("device/{deviceId}/current")]
        public async Task<ActionResult<EnergyUsageDto>> GetCurrentUsage(int deviceId)
        {
            return await _energyUsageService.GetCurrentUsageAsync(deviceId);
        }

        [HttpGet("device/{deviceId}/history")]
        public async Task<ActionResult<IEnumerable<EnergyUsageDto>>> GetUsageHistory(
            int deviceId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var utcStartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            var utcEndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);
            var history = await _energyUsageService.GetUsageHistoryAsync(deviceId, startDate, endDate);
            return Ok(history);
        }

        [HttpPost]
        public async Task<ActionResult<EnergyUsageDto>> Create(CreateEnergyUsageDto createEnergyUsageDto)
        {
            var usage = await _energyUsageService.AddAsync(createEnergyUsageDto);
            return CreatedAtAction(nameof(GetById), new { id = usage.UsageId }, usage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _energyUsageService.RemoveAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}