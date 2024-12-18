using System.Collections.Generic;

namespace EnergyManagementSystem.Core.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }  
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public bool PowerSavingMode { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public House House { get; set; }
        public Room Room { get; set; }
        public ICollection<EnergyUsage> EnergyUsages { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Limit> Limits { get; set; }
    }
}
