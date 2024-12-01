using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace EnergyManagementSystem.Data.Repositories
{
    public class EnergyUsageRepository : GenericRepository<EnergyUsage>, IEnergyUsageRepository
    {
        private readonly DatabaseContext _context;

        public EnergyUsageRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnergyUsage>> GetUsageByDeviceIdAsync(int deviceId, DateTime startDate, DateTime endDate)
        {
            return await _context.EnergyUsages
                .Where(e => e.DeviceId == deviceId
                        && e.Timestamp >= startDate
                        && e.Timestamp <= endDate)
                .Include(e => e.Device)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalConsumptionAsync(int deviceId, DateTime startDate, DateTime endDate)
        {
            return await _context.EnergyUsages
                .Where(e => e.DeviceId == deviceId
                        && e.Timestamp >= startDate
                        && e.Timestamp <= endDate)
                .SumAsync(e => e.Consumption);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.EnergyUsages.FindAsync(id);
            if (entity == null)
                return false;

            _context.EnergyUsages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
