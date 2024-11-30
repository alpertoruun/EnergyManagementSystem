using System.ComponentModel.DataAnnotations;

namespace EnergyManagementSystem.Core.Models
{
    public class EnergyUsage
    {
        [Key]
        public int UsageId { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }

        // Navigation property
        public Device Device { get; set; }
    }
}
