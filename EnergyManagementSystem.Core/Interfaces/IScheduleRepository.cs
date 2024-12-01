using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByDeviceIdAsync(int deviceId);
        Task<IEnumerable<Schedule>> GetActiveSchedulesAsync();
    }
}
