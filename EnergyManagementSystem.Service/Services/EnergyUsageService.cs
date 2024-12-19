using EnergyManagementSystem.Core.DTOs.EnergyUsage;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using AutoMapper;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.DTOs.Notification;

namespace EnergyManagementSystem.Service.Services
{
    public class EnergyUsageService : IEnergyUsageService
    {
        private readonly IEnergyUsageRepository _energyUsageRepository;
        private readonly ILimitService _limitService;
        private readonly IDeviceService _deviceService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public EnergyUsageService(
        IEnergyUsageRepository energyUsageRepository,
        ILimitService limitService,
        IDeviceService deviceService,
        INotificationService notificationService,
        IMapper mapper)
        {
            _energyUsageRepository = energyUsageRepository;
            _limitService = limitService;
            _deviceService = deviceService;
            _notificationService = notificationService;
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
            // İlgili cihaza ait en son limiti al
            var deviceLimits = await _limitService.GetLimitsByDeviceAsync(dto.DeviceId);
            var latestLimit = deviceLimits.OrderByDescending(l => l.LimitId).FirstOrDefault();

            if (latestLimit != null)
            {
                var startDate = GetStartDateByPeriod(latestLimit.Period);
                var endDate = DateTime.UtcNow;

                var usageHistory = await GetUsageHistoryAsync(dto.DeviceId, startDate, endDate);
                var totalUsage = usageHistory.Sum(u => u.Consumption);

                // Yeni eklenecek değeri de hesaba kat
                if (totalUsage + dto.Consumption > latestLimit.LimitValue)
                {
                    // Cihazı kapat
                    await _deviceService.UpdateDeviceStatusAsync(dto.DeviceId, false);

                    // Bildirim gönder
                    var device = await _deviceService.GetByIdAsync(dto.DeviceId);
                    await CreateNotification(device,
                        $"{device.Name} cihazı için limit seviyesine ulaşıldı. Cihaz kapatıldı. " +
                        $"Mevcut kullanım: {totalUsage + dto.Consumption}, Limit: {latestLimit.LimitValue} {latestLimit.LimitType}");

                    // Limit aşıldığı için yeni veriyi eklemeyi reddet
                    throw new InvalidOperationException(
                        $"Enerji limit seviyesine ulaşıldı.Mevcut kullanım : {totalUsage + dto.Consumption}, " +
                        $"Limit: {latestLimit.LimitValue} {latestLimit.LimitType} / {latestLimit.Period}");
                }
            }

            // Limit yoksa veya aşılmadıysa veriyi ekle
            var entity = _mapper.Map<EnergyUsage>(dto);
            await _energyUsageRepository.AddAsync(entity);
            return _mapper.Map<EnergyUsageDto>(entity);
        }
        private DateTime GetStartDateByPeriod(string period)
        {
            var currentDate = DateTime.UtcNow;
            return period.ToLower() switch
            {
                "daily" => currentDate.Date,
                "weekly" => currentDate.AddDays(-7),
                "monthly" => currentDate.AddMonths(-1),
                "yearly" => currentDate.AddYears(-1),
                _ => currentDate.Date
            };
        }

        private async Task CreateNotification(DeviceDto device, string message)
        {
            var userId = await _deviceService.GetUserIdByDeviceIdAsync(device.DeviceId);
            if (userId == null)
            {
                throw new Exception("UserId could not be found for the given device.");
            }
            var notification = new CreateNotificationDto
            {
                UserId = userId.Value,
                Message = message,
                Type = "Limit"
            };

            await _notificationService.CreateAsync(notification);
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
            var endTime = DateTime.UtcNow;
            var startTime = endTime.AddMinutes(-5);

            var usages = await _energyUsageRepository.GetUsageByDeviceIdAsync(
                deviceId,
                startTime,
                endTime
            );

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