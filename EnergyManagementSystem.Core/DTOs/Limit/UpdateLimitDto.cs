using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Limit
{
    public class UpdateLimitDto
    {
        public string LimitType { get; set; }
        public decimal LimitValue { get; set; }
        public string Period { get; set; }
    }
}
