using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IEnergyUsageRepository : IGenericRepository<EnergyUsage>
    {
        Task<IEnumerable<EnergyUsage>> GetUsageByDeviceIdAsync(int deviceId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalConsumptionAsync(int deviceId, DateTime startDate, DateTime endDate);
        Task<bool> RemoveAsync(int id);
    }
}
