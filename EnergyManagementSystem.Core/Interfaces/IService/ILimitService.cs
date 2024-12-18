using EnergyManagementSystem.Core.DTOs.Limit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces.IService
{
    public interface ILimitService
    {
        Task<LimitDto> GetByIdAsync(int limitId);
        Task<IEnumerable<LimitDto>> GetLimitsByDeviceAsync(int deviceId);
        Task<LimitDto> CreateAsync(CreateLimitDto createLimitDto);
        Task UpdateAsync(int limitId, UpdateLimitDto updateLimitDto);
        Task DeleteAsync(int limitId);
    }
}
