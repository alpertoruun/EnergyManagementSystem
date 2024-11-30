namespace EnergyManagementSystem.Core.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }  // "Warning", "Info" etc.
        public bool Status { get; set; }  // true = okundu, false = okunmadı
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
