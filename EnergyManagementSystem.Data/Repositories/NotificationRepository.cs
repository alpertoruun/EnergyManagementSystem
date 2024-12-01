using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Data.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly DatabaseContext _context;

        public NotificationRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.Status)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetReadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.Status)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.Status);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.Status = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
