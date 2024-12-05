using EnergyManagementSystem.Core.DTOs.Notification;
using EnergyManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            return await _notificationService.GetByIdAsync(id);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUserNotifications(int userId)
        {
            return Ok(await _notificationService.GetUserNotificationsAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationDto createNotificationDto)
        {
            await _notificationService.CreateAsync(createNotificationDto);
            return Ok();
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("user/{userId}/unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            return await _notificationService.GetUnreadCountAsync(userId);
        }
    }
}
