using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetById(int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            return device;
        }

        [HttpGet("house/{houseId}")]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetHouseDevices(int houseId)
        {
            var devices = await _deviceService.GetHouseDevicesAsync(houseId);
            return Ok(devices);
        }

        [HttpGet("room/{roomId}")]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetRoomDevices(int roomId)
        {
            var devices = await _deviceService.GetDevicesByRoomAsync(roomId);
            return Ok(devices);
        }

        [HttpPost("{houseId}")]
        public async Task<ActionResult<DeviceDto>> Create(int houseId, CreateDeviceDto createDeviceDto)
        {
            var device = await _deviceService.CreateAsync(houseId, createDeviceDto);
            return CreatedAtAction(nameof(GetById), new { id = device.DeviceId }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDeviceDto updateDeviceDto)
        {
            await _deviceService.UpdateAsync(id, updateDeviceDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _deviceService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool status)
        {
            await _deviceService.UpdateDeviceStatusAsync(id, status);
            return NoContent();
        }
    }
}
