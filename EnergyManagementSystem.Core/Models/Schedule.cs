namespace EnergyManagementSystem.Core.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int DeviceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Repeat { get; set; }  // "daily", "weekly" etc.

        // Navigation property
        public Device Device { get; set; }
    }
}
