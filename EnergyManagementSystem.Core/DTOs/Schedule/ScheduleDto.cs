using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.Schedule
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public int DeviceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Repeat { get; set; }
    }

}
