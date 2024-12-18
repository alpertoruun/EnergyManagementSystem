using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface ILimitRepository : IGenericRepository<Limit>
    {
        Task<IEnumerable<Limit>> GetLimitsByDeviceIdAsync(int deviceId);
        Task<Limit> GetLimitWithDetailsAsync(int limitId);
    }
}
