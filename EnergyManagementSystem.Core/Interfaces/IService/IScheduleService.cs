using EnergyManagementSystem.Core.DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IScheduleService 
    { Task<ScheduleDto> GetByIdAsync(int scheduleId);
        Task<IEnumerable<ScheduleDto>> GetDeviceSchedulesAsync(int deviceId);
        Task<ScheduleDto> CreateAsync(CreateScheduleDto createScheduleDTO);
        Task UpdateAsync(int scheduleId, UpdateScheduleDto updateScheduleDTO);
        Task DeleteAsync(int scheduleId);
        Task<IEnumerable<ScheduleDto>> GetActiveSchedulesAsync(int deviceId);
        
    }
}
