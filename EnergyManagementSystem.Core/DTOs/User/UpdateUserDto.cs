using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.User
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
