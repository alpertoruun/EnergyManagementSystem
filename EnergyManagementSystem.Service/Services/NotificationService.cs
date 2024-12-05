using EnergyManagementSystem.Core.DTOs.Notification;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<NotificationDto> GetByIdAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            return MapToNotificationDto(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
            var notificationDtos = new List<NotificationDto>();

            foreach (var notification in notifications)
            {
                notificationDtos.Add(MapToNotificationDto(notification));
            }

            return notificationDtos;
        }

        public async Task CreateAsync(CreateNotificationDto createNotificationDto)
        {
            var user = await _userRepository.GetByIdAsync(createNotificationDto.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {createNotificationDto.UserId} not found.");

            var notification = new Notification
            {
                UserId = createNotificationDto.UserId,
                Message = createNotificationDto.Message,
                Type = createNotificationDto.Type,
                Status = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            notification.Status = true; 
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            await _notificationRepository.DeleteAsync(notification);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return await _notificationRepository.GetUnreadNotificationCountAsync(userId);
        }

        private NotificationDto MapToNotificationDto(Notification notification)
        {
            return new NotificationDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Message = notification.Message,
                Type = notification.Type,
                Status = notification.Status,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}