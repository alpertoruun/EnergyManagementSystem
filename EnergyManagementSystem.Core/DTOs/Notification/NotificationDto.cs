using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
