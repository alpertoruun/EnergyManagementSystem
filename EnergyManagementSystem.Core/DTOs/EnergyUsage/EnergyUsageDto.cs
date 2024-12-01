using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.EnergyUsage
{
    public class EnergyUsageDto
    {
        public int UsageId { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Consumption { get; set; }
    }

}
