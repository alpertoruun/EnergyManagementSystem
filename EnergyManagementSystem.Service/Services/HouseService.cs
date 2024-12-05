using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IHouseRepository _houseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDeviceRepository _deviceRepository;

        public HouseService(
            IHouseRepository houseRepository,
            IUserRepository userRepository,
            IDeviceRepository deviceRepository)
        {
            _houseRepository = houseRepository ?? throw new ArgumentNullException(nameof(houseRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task<HouseDto> GetByIdAsync(int houseId)
        {
            var house = await _houseRepository.GetHouseWithDetailsAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            return MapToHouseDto(house);
        }

        public async Task<IEnumerable<HouseDto>> GetUserHousesAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var houses = await _houseRepository.GetHousesByUserIdAsync(userId);
            var houseDtos = new List<HouseDto>();

            foreach (var house in houses)
            {
                houseDtos.Add(MapToHouseDto(house));
            }

            return houseDtos;
        }

        public async Task<HouseDto> CreateAsync(int userId, CreateHouseDto createHouseDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var house = new House
            {
                UserId = userId,
                Name = createHouseDto.Name,
                Address = createHouseDto.Address,
                PowerSavingMode = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdHouse = await _houseRepository.AddAsync(house);
            return await GetByIdAsync(createdHouse.HouseId);
        }

        public async Task UpdateAsync(int houseId, UpdateHouseDto updateHouseDto)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            house.Name = updateHouseDto.Name;
            house.Address = updateHouseDto.Address;
            house.PowerSavingMode = updateHouseDto.PowerSavingMode;

            await _houseRepository.UpdateAsync(house);

            // If PowerSavingMode is enabled/disabled, update all devices in the house
            if (house.PowerSavingMode != updateHouseDto.PowerSavingMode)
            {
                var devices = await _deviceRepository.GetDevicesByHouseIdAsync(houseId);
                foreach (var device in devices)
                {
                    device.PowerSavingMode = updateHouseDto.PowerSavingMode;
                    if (updateHouseDto.PowerSavingMode)
                    {
                        device.Status = false; // Turn off devices when power saving is enabled
                    }
                    await _deviceRepository.UpdateAsync(device);
                }
            }
        }

        public async Task DeleteAsync(int houseId)
        {
            var house = await _houseRepository.GetHouseWithDetailsAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            // Check if house has any devices
            if (house.Devices?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete house with associated devices. Please remove all devices first.");
            }

            // Check if house has any rooms
            if (house.Rooms?.Any() == true)
            {
                throw new InvalidOperationException("Cannot delete house with associated rooms. Please remove all rooms first.");
            }

            await _houseRepository.DeleteAsync(house);
        }

        public async Task TogglePowerSavingModeAsync(int houseId, bool enabled)
        {
            var house = await _houseRepository.GetByIdAsync(houseId);
            if (house == null)
                throw new KeyNotFoundException($"House with ID {houseId} not found.");

            house.PowerSavingMode = enabled;
            await _houseRepository.UpdateAsync(house);

            // Update all devices in the house
            var devices = await _deviceRepository.GetDevicesByHouseIdAsync(houseId);
            foreach (var device in devices)
            {
                device.PowerSavingMode = enabled;
                if (enabled)
                {
                    device.Status = false; // Turn off devices when power saving is enabled
                }
                await _deviceRepository.UpdateAsync(device);
            }
        }

        private HouseDto MapToHouseDto(House house)
        {
            return new HouseDto
            {
                HouseId = house.HouseId,
                UserId = house.UserId,
                Name = house.Name,
                Address = house.Address,
                PowerSavingMode = house.PowerSavingMode
            };
        }
    }
}