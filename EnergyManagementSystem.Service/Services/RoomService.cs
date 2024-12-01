using EnergyManagementSystem.Core.DTOs.Room;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHouseRepository _houseRepository;

        public RoomService(IRoomRepository roomRepository, IHouseRepository houseRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _houseRepository = houseRepository ?? throw new ArgumentNullException(nameof(houseRepository));
        }

        public async Task<RoomDto> GetByIdAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");

            return new RoomDto
            {
                RoomId = room.RoomId,
                HouseId = room.HouseId,
                Name = room.Name
            };
        }

        public async Task<IEnumerable<RoomDto>> GetHouseRoomsAsync(int houseId)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            var rooms = await _roomRepository.GetRoomsByHouseIdAsync(houseId);
            var roomDtos = new List<RoomDto>();

            foreach (var room in rooms)
            {
                roomDtos.Add(new RoomDto
                {
                    RoomId = room.RoomId,
                    HouseId = room.HouseId,
                    Name = room.Name
                });
            }

            return roomDtos;
        }

        public async Task<RoomDto> CreateAsync(CreateRoomDto createRoomDto)
        {
            var house = await _houseRepository.GetByIdAsync(createRoomDto.HouseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {createRoomDto.HouseId} not found.");

            var room = new Room
            {
                HouseId = createRoomDto.HouseId,
                Name = createRoomDto.Name
            };

            var createdRoom = await _roomRepository.AddAsync(room);

            return new RoomDto
            {
                RoomId = createdRoom.RoomId,
                HouseId = createdRoom.HouseId,
                Name = createdRoom.Name
            };
        }

        public async Task UpdateAsync(int roomId, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");

            room.Name = updateRoomDto.Name;

            await _roomRepository.UpdateAsync(room);
        }

        public async Task DeleteAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");

            var roomWithDevices = await _roomRepository.GetRoomWithDevicesAsync(roomId);
            if (roomWithDevices.Devices?.Any() == true)
                throw new InvalidOperationException("Cannot delete room with associated devices.");

            await _roomRepository.DeleteAsync(room);
        }

        public async Task<RoomDto> GetRoomWithDevicesAsync(int roomId)
        {
            var room = await _roomRepository.GetRoomWithDevicesAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");

            return new RoomDto
            {
                RoomId = room.RoomId,
                HouseId = room.HouseId,
                Name = room.Name
            };
        }
    }
}