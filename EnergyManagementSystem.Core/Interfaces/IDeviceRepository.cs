using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        Task<IEnumerable<Device>> GetDevicesByHouseIdAsync(int houseId);
        Task<IEnumerable<Device>> GetDevicesByRoomIdAsync(int roomId);
        Task<Device> GetDeviceWithDetailsAsync(int deviceId);
    }

}
