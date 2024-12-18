using EnergyManagementSystem.Core.DTOs.Limit;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Service.Services
{
    public class LimitService : ILimitService
    {
        private readonly ILimitRepository _limitRepository;
        private readonly IDeviceRepository _deviceRepository;

        public LimitService(
            ILimitRepository limitRepository,
            IDeviceRepository deviceRepository)
        {
            _limitRepository = limitRepository ?? throw new ArgumentNullException(nameof(limitRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task<LimitDto> GetByIdAsync(int limitId)
        {
            var limit = await _limitRepository.GetLimitWithDetailsAsync(limitId);
            if (limit == null)
                throw new KeyNotFoundException($"Limit with ID {limitId} not found.");

            return MapToLimitDto(limit);
        }

        public async Task<IEnumerable<LimitDto>> GetLimitsByDeviceAsync(int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            var limits = await _limitRepository.GetLimitsByDeviceIdAsync(deviceId);
            return limits.Select(MapToLimitDto);
        }

        public async Task<LimitDto> CreateAsync(CreateLimitDto createLimitDto)
        {
            var device = await _deviceRepository.GetByIdAsync(createLimitDto.DeviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {createLimitDto.DeviceId} not found.");

            var limit = new Limit
            {
                DeviceId = createLimitDto.DeviceId,
                LimitType = createLimitDto.LimitType,
                LimitValue = createLimitDto.LimitValue,
                Period = createLimitDto.Period
            };

            var createdLimit = await _limitRepository.AddAsync(limit);
            return await GetByIdAsync(createdLimit.LimitId);
        }

        public async Task UpdateAsync(int limitId, UpdateLimitDto updateLimitDto)
        {
            var limit = await _limitRepository.GetByIdAsync(limitId);
            if (limit == null)
                throw new KeyNotFoundException($"Limit with ID {limitId} not found.");

            limit.LimitType = updateLimitDto.LimitType;
            limit.LimitValue = updateLimitDto.LimitValue;
            limit.Period = updateLimitDto.Period;

            await _limitRepository.UpdateAsync(limit);
        }

        public async Task DeleteAsync(int limitId)
        {
            var limit = await _limitRepository.GetByIdAsync(limitId);
            if (limit == null)
                throw new KeyNotFoundException($"Limit with ID {limitId} not found.");

            await _limitRepository.DeleteAsync(limit);
        }

        private LimitDto MapToLimitDto(Limit limit)
        {
            return new LimitDto
            {
                LimitId = limit.LimitId,
                DeviceId = limit.DeviceId,
                LimitType = limit.LimitType,
                LimitValue = limit.LimitValue,
                Period = limit.Period,
            };
        }
    }
}
