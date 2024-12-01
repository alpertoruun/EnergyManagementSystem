using EnergyManagementSystem.Core.DTOs.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IHouseService 
    { Task<HouseDto> GetByIdAsync(int houseId);
        Task<IEnumerable<HouseDto>> GetUserHousesAsync(int userId);
        Task<HouseDto> CreateAsync(int userId, CreateHouseDto createHouseDTO);
        Task UpdateAsync(int houseId, UpdateHouseDto updateHouseDTO);
        Task DeleteAsync(int houseId);
        Task TogglePowerSavingModeAsync(int houseId, bool enabled);
        
    }
}
