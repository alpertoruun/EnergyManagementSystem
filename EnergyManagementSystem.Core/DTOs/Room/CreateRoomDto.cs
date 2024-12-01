using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Room
{
    public class CreateRoomDto
    {
        public int HouseId { get; set; }
        public string Name { get; set; }
    }
}
