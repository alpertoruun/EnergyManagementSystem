using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.DTOs.UserSetting;
using EnergyManagementSystem.Core.Interfaces;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsService _userSettingsService;

        public UserSettingsController(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserSettingDto>> GetAllSettings(int userId)
        {
            return await _userSettingsService.GetAllSettingsAsync(userId);
        }

        [HttpGet("{userId}/preference/{key}")]
        public async Task<ActionResult<string>> GetPreference(int userId, string key)
        {
            var value = await _userSettingsService.GetPreferenceAsync(userId, key);
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        [HttpPut("{userId}/preference/{key}")]
        public async Task<IActionResult> UpdateSetting(int userId, string key, [FromBody] string value)
        {
            await _userSettingsService.UpdateSettingAsync(userId, key, value);
            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateSettings(int userId, UpdateUserSettingDto settings)
        {
            await _userSettingsService.UpdateSettingsAsync(userId, settings);
            return NoContent();
        }
    }
}