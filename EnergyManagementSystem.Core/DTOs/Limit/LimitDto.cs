using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Limit
{
    public class LimitDto
    {
        public int LimitId { get; set; }
        public int DeviceId { get; set; }
        public string LimitType { get; set; }
        public decimal LimitValue { get; set; }
        public string Period { get; set; }
    }
}
