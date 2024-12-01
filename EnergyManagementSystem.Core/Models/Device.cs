using EnergyManagementSystem.Core.Enums;
using System.Collections.Generic;

namespace EnergyManagementSystem.Core.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }  
        public string Name { get; set; }
        public DeviceStatus Status { get; set; }
        public DeviceType Type { get; set; }
        public bool PowerSavingMode { get; set; }
        public decimal? EnergyLimit { get; set; }  // Opsiyonel
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public House House { get; set; }
        public Room Room { get; set; }
        public ICollection<EnergyUsage> EnergyUsages { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Limit> Limits { get; set; }
    }
}
