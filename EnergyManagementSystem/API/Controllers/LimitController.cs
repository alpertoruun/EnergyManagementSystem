using EnergyManagementSystem.Core.DTOs.Limit;
using EnergyManagementSystem.Core.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LimitsController : ControllerBase
    {
        private readonly ILimitService _limitService;

        public LimitsController(ILimitService limitService)
        {
            _limitService = limitService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LimitDto>> GetById(int id)
        {
            var limit = await _limitService.GetByIdAsync(id);
            return limit;
        }

        [HttpGet("device/{deviceId}")]
        public async Task<ActionResult<IEnumerable<LimitDto>>> GetDeviceLimits(int deviceId)
        {
            var limits = await _limitService.GetLimitsByDeviceAsync(deviceId);
            return Ok(limits);
        }

        [HttpPost]
        public async Task<ActionResult<LimitDto>> Create(CreateLimitDto createLimitDto)
        {
            var limit = await _limitService.CreateAsync(createLimitDto);
            return CreatedAtAction(nameof(GetById), new { id = limit.LimitId }, limit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateLimitDto updateLimitDto)
        {
            await _limitService.UpdateAsync(id, updateLimitDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _limitService.DeleteAsync(id);
            return NoContent();
        }
    }
}