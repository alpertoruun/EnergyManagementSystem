using EnergyManagementSystem.Core.DTOs;
using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.Models;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces.IService
{
    public interface IDeviceService
    {
        Task<DeviceDto> GetByIdAsync(int deviceId);
        Task<IEnumerable<DeviceDto>> GetHouseDevicesAsync(int houseId);
        Task<DeviceDto> CreateAsync(int houseId, CreateDeviceDto createDeviceDTO);
        Task UpdateAsync(int deviceId, UpdateDeviceDto updateDeviceDTO);
        Task DeleteAsync(int deviceId);
        Task<IEnumerable<DeviceDto>> GetDevicesByRoomAsync(int roomId);
        Task UpdateDeviceStatusAsync(int deviceId, bool status);
        Task<int?> GetUserIdByDeviceIdAsync(int deviceId);
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
    }
}
