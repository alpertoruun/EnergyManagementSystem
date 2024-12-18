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
    public class LimitRepository : GenericRepository<Limit>, ILimitRepository
    {
        private readonly DatabaseContext _context;

        public LimitRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Limit>> GetLimitsByDeviceIdAsync(int deviceId)
        {
            return await _context.Limits
                .Where(l => l.DeviceId == deviceId)
                .Include(l => l.Device)
                .ToListAsync();
        }

        public async Task<Limit> GetLimitWithDetailsAsync(int limitId)
        {
            return await _context.Limits
                .Include(l => l.Device)
                .FirstOrDefaultAsync(l => l.LimitId == limitId);
        }
    }
}
