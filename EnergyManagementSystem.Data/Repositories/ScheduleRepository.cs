using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Data.Repositories
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        private readonly DatabaseContext _context;

        public ScheduleRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByDeviceIdAsync(int deviceId)
        {
            return await _context.Schedules
                .Where(s => s.DeviceId == deviceId)
                .Include(s => s.Device)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetActiveSchedulesAsync()
        {
            var currentTime = DateTime.Now;
            return await _context.Schedules
                .Where(s => s.StartTime <= currentTime && s.EndTime >= currentTime)
                .Include(s => s.Device)
                .ToListAsync();
        }
    }
}
