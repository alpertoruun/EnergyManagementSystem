using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsByHouseIdAsync(int houseId);
        Task<Room> GetRoomWithDevicesAsync(int roomId);
    }
}
