using EnergyManagementSystem.Core.DTOs.EnergyUsage;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using AutoMapper;

namespace EnergyManagementSystem.Service.Services
{
    public class EnergyUsageService : IEnergyUsageService
    {
        private readonly IEnergyUsageRepository _energyUsageRepository;
        private readonly IMapper _mapper;

        public EnergyUsageService(IEnergyUsageRepository energyUsageRepository, IMapper mapper)
        {
            _energyUsageRepository = energyUsageRepository;
            _mapper = mapper;
        }

        public async Task<EnergyUsageDto> GetByIdAsync(int id)
        {
            var entity = await _energyUsageRepository.GetByIdAsync(id);
            return _mapper.Map<EnergyUsageDto>(entity);
        }

        public async Task<IEnumerable<EnergyUsageDto>> GetAllAsync()
        {
            var entities = await _energyUsageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EnergyUsageDto>>(entities);
        }

        public async Task<EnergyUsageDto> AddAsync(CreateEnergyUsageDto dto)
        {
            var entity = _mapper.Map<EnergyUsage>(dto);
            await _energyUsageRepository.AddAsync(entity);
            return _mapper.Map<EnergyUsageDto>(entity);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            return await _energyUsageRepository.RemoveAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, CreateEnergyUsageDto dto)
        {
            var existingEntity = await _energyUsageRepository.GetByIdAsync(id);
            if (existingEntity == null)
                return false;

            _mapper.Map(dto, existingEntity);
            await _energyUsageRepository.UpdateAsync(existingEntity);
            return true;
        }

        public async Task<EnergyUsageDto> GetCurrentUsageAsync(int deviceId)
        {
            var usages = await _energyUsageRepository.GetUsageByDeviceIdAsync(deviceId, DateTime.Now.AddDays(-1), DateTime.Now);
            var lastUsage = usages.OrderByDescending(u => u.Timestamp).FirstOrDefault();
            return _mapper.Map<EnergyUsageDto>(lastUsage);
        }

        public async Task<IEnumerable<EnergyUsageDto>> GetUsageHistoryAsync(int deviceId, DateTime startDate, DateTime endDate)
        {
            var history = await _energyUsageRepository.GetUsageByDeviceIdAsync(deviceId, startDate, endDate);
            return _mapper.Map<IEnumerable<EnergyUsageDto>>(history);
        }
    }
}