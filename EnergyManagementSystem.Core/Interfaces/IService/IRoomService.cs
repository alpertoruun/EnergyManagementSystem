using EnergyManagementSystem.Core.DTOs.Room;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IRoomService
    {
        Task<RoomDto> GetByIdAsync(int roomId);
        Task<IEnumerable<RoomDto>> GetHouseRoomsAsync(int houseId);
        Task<RoomDto> CreateAsync(CreateRoomDto createRoomDto);
        Task UpdateAsync(int roomId, UpdateRoomDto updateRoomDto);
        Task DeleteAsync(int roomId);
        Task<RoomDto> GetRoomWithDevicesAsync(int roomId);
    }
}