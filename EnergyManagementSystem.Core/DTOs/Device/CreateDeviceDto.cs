using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnergyManagementSystem.Core.DTOs.Device
{
    public class CreateDeviceDto
    {
        public string Name { get; set; }

        public string Type { get; set; }  
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public bool PowerSavingMode { get; set; }
    }

}
