using EnergyManagementSystem.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Device
{
    public class UpdateDeviceDto
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public bool PowerSavingMode { get; set; }
        public decimal? EnergyLimit { get; set; }
        public string? LimitType { get; set; }
    }

}
