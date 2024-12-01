using EnergyManagementSystem.Core.DTOs.EnergyUsage;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IEnergyUsageService
    {
        Task<EnergyUsageDto> GetByIdAsync(int id);
        Task<IEnumerable<EnergyUsageDto>> GetAllAsync();
        Task<EnergyUsageDto> AddAsync(CreateEnergyUsageDto dto);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateAsync(int id, CreateEnergyUsageDto dto);

        Task<EnergyUsageDto> GetCurrentUsageAsync(int deviceId);
        Task<IEnumerable<EnergyUsageDto>> GetUsageHistoryAsync(int deviceId, DateTime startDate, DateTime endDate);
    }
}
