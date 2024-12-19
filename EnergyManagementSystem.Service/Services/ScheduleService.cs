using EnergyManagementSystem.Core.DTOs.Schedule;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IDeviceRepository _deviceRepository;

        public ScheduleService(
            IScheduleRepository scheduleRepository,
            IDeviceRepository deviceRepository)
        {
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }

        public async Task<ScheduleDto> GetByIdAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            return MapToScheduleDto(schedule);
        }

        public async Task<IEnumerable<ScheduleDto>> GetDeviceSchedulesAsync(int deviceId)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {deviceId} not found.");

            var schedules = await _scheduleRepository.GetSchedulesByDeviceIdAsync(deviceId);
            var scheduleDtos = new List<ScheduleDto>();

            foreach (var schedule in schedules)
            {
                scheduleDtos.Add(MapToScheduleDto(schedule));
            }

            return scheduleDtos;
        }

        public async Task<ScheduleDto> CreateAsync(CreateScheduleDto createScheduleDto)
        {
            var device = await _deviceRepository.GetByIdAsync(createScheduleDto.DeviceId);
            if (device == null)
                throw new KeyNotFoundException($"Device with ID {createScheduleDto.DeviceId} not found.");

            // Zaman kontrolü
            if (createScheduleDto.StartTime >= createScheduleDto.EndTime)
                throw new InvalidOperationException("Start time must be before end time.");

            var schedule = new Schedule
            {
                DeviceId = createScheduleDto.DeviceId,
                StartTime = createScheduleDto.StartTime,
                EndTime = createScheduleDto.EndTime,
                Repeat = createScheduleDto.Repeat
            };

            var createdSchedule = await _scheduleRepository.AddAsync(schedule);
            return MapToScheduleDto(createdSchedule);
        }

        public async Task UpdateAsync(int scheduleId, UpdateScheduleDto updateScheduleDto)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            // Zaman kontrolü
            if (updateScheduleDto.StartTime >= updateScheduleDto.EndTime)
                throw new InvalidOperationException("Start time must be before end time.");

            schedule.StartTime = updateScheduleDto.StartTime;
            schedule.EndTime = updateScheduleDto.EndTime;
            schedule.Repeat = updateScheduleDto.Repeat;

            await _scheduleRepository.UpdateAsync(schedule);
        }

        public async Task DeleteAsync(int scheduleId)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new KeyNotFoundException($"Schedule with ID {scheduleId} not found.");

            await _scheduleRepository.DeleteAsync(schedule);
        }

        public async Task<IEnumerable<ScheduleDto>> GetActiveSchedulesAsync(int deviceId)
        {
            var currentTime = DateTime.UtcNow;
            var schedules = await _scheduleRepository.GetSchedulesByDeviceIdAsync(deviceId);

            return schedules
                .Where(s =>
                    (!string.IsNullOrEmpty(s.Repeat)) || // Tekrarlı olanlar her zaman gelsin
                    (string.IsNullOrEmpty(s.Repeat) && // Tekrarsız olanlar için
                     s.EndTime >= currentTime.AddMinutes(-1))) // End date'i henüz geçmemiş olanlar (1 dk tolerans)
                .Select(MapToScheduleDto);
        }

        private ScheduleDto MapToScheduleDto(Schedule schedule)
        {
            return new ScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                DeviceId = schedule.DeviceId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Repeat = schedule.Repeat
            };
        }
    }
}