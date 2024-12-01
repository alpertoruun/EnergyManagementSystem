using EnergyManagementSystem.Core.DTOs.Notification;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface INotificationService 
    { Task<NotificationDto> GetByIdAsync(int notificationId);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task CreateAsync(CreateNotificationDto createNotificationDTO);
        Task MarkAsReadAsync(int notificationId);
        Task DeleteAsync(int notificationId);
        Task<int> GetUnreadCountAsync(int userId);
        
    }
}
