using EnergyManagementSystem.Core.DTOs.Schedule;
using EnergyManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetById(int id)
        {
            return await _scheduleService.GetByIdAsync(id);
        }

        [HttpGet("device/{deviceId}")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetDeviceSchedules(int deviceId)
        {
            return Ok(await _scheduleService.GetDeviceSchedulesAsync(deviceId));
        }

        [HttpGet("device/{deviceId}/active")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetActiveSchedules(int deviceId)
        {
            return Ok(await _scheduleService.GetActiveSchedulesAsync(deviceId));
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> Create(CreateScheduleDto createScheduleDto)
        {
            var schedule = await _scheduleService.CreateAsync(createScheduleDto);
            return CreatedAtAction(nameof(GetById), new { id = schedule.ScheduleId }, schedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateScheduleDto updateScheduleDto)
        {
            await _scheduleService.UpdateAsync(id, updateScheduleDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _scheduleService.DeleteAsync(id);
            return NoContent();
        }
    }
}
