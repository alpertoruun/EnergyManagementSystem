using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.EnergyUsage
{
    public class CreateEnergyUsageDto
    {
        public int DeviceId { get; set; }
        public decimal Consumption { get; set; }
    }
}
