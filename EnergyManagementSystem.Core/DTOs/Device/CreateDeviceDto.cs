using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyManagementSystem.Core.Enums;


namespace EnergyManagementSystem.Core.DTOs.Device
{
    public class CreateDeviceDto
    {
        public string Name { get; set; }

        public DeviceType Type { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public bool PowerSavingMode { get; set; }
    }

}
