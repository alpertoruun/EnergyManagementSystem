using EnergyManagementSystem.Core.DTOs;
using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.Enums;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IHouseRepository _houseRepository;
        private readonly IRoomRepository _roomRepository;

        public DeviceService(
            IDeviceRepository deviceRepository,
            IHouseRepository houseRepository,
            IRoomRepository roomRepository)
        {
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            _houseRepository = houseRepository ?? throw new ArgumentNullException(nameof(houseRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        public async Task<DeviceDto> GetByIdAsync(int deviceId)
        {
            var device = await _deviceRepository.GetDeviceWithDetailsAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            return MapToDeviceDto(device);
        }

        public async Task<IEnumerable<DeviceDto>> GetHouseDevicesAsync(int houseId)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            var devices = await _deviceRepository.GetDevicesByHouseIdAsync(houseId);
            var deviceDtos = new List<DeviceDto>();

            foreach (var device in devices)
            {
                deviceDtos.Add(MapToDeviceDto(device));
            }

            return deviceDtos;
        }

        public async Task<DeviceDto> CreateAsync(int houseId, CreateDeviceDto createDeviceDto)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            var room = await _roomRepository.GetByIdAsync(createDeviceDto.RoomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {createDeviceDto.RoomId} not found.");

            if (room.HouseId != houseId)
                throw new InvalidOperationException("Room does not belong to the specified house.");

            var device = new Device
            {
                Name = createDeviceDto.Name,
                Type = createDeviceDto.Type,
                HouseId = houseId,
                RoomId = createDeviceDto.RoomId,
                PowerSavingMode = createDeviceDto.PowerSavingMode,
                Status = DeviceStatus.Off 
            };

            var createdDevice = await _deviceRepository.AddAsync(device);
            return await GetByIdAsync(createdDevice.DeviceId);
        }

        public async Task UpdateAsync(int deviceId, UpdateDeviceDto updateDeviceDto)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            device.Name = updateDeviceDto.Name;
            device.Type = updateDeviceDto.Type;

            // PowerSaving mode açıksa status'ü PowerSaving yapıyoruz
            if (updateDeviceDto.PowerSavingMode)
            {
                device.Status = DeviceStatus.PowerSaving;
            }
            // PowerSaving mode kapalıysa DTO'dan gelen status'ü kullanıyoruz
            else
            {
                // PowerSaving modunda değilse sadece On veya Off olabilir
                device.Status = updateDeviceDto.Status == DeviceStatus.PowerSaving ?
                    DeviceStatus.Off : updateDeviceDto.Status;
            }
            device.PowerSavingMode = updateDeviceDto.PowerSavingMode;
            device.EnergyLimit = updateDeviceDto.EnergyLimit;

            await _deviceRepository.UpdateAsync(device);
        }

        public async Task DeleteAsync(int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            await _deviceRepository.DeleteAsync(device);
        }

        public async Task<IEnumerable<DeviceDto>> GetDevicesByRoomAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");

            var devices = await _deviceRepository.GetDevicesByRoomIdAsync(roomId);
            var deviceDtos = new List<DeviceDto>();

            foreach (var device in devices)
            {
                deviceDtos.Add(MapToDeviceDto(device));
            }

            return deviceDtos;
        }

        public async Task UpdateDeviceStatusAsync(int deviceId, DeviceStatus status)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            device.Status = status;

            // PowerSaving moduna geçilirse PowerSavingMode'u true yapıyoruz
            if (status == DeviceStatus.PowerSaving)
            {
                device.PowerSavingMode = true;
            }
            // PowerSaving modundan çıkılırsa PowerSavingMode'u false yapıyoruz
            else if (device.PowerSavingMode && (status == DeviceStatus.On || status == DeviceStatus.Off))
            {
                device.PowerSavingMode = false;
            }

            await _deviceRepository.UpdateAsync(device);
        }

        private DeviceDto MapToDeviceDto(Device device)
        {
            return new DeviceDto
            {
                DeviceId = device.DeviceId,
                Name = device.Name,
                Type = device.Type,
                Status = device.Status, 
                PowerSavingMode = device.PowerSavingMode,
                EnergyLimit = device.EnergyLimit,
                RoomName = device.Room?.Name,
                HouseName = device.House?.Name
            };
        }
    }
}