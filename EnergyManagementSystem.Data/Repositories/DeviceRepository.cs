using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace EnergyManagementSystem.Data.Repositories
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        private readonly DatabaseContext _context;

        public DeviceRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Device>> GetDevicesByHouseIdAsync(int houseId)
        {
            return await _context.Devices
                .Where(d => d.HouseId == houseId)
                .Include(d => d.Room)
                .Include(d => d.Limits)  
                .ToListAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByRoomIdAsync(int roomId)
        {
            return await _context.Devices
                .Where(d => d.RoomId == roomId)
                .Include(d => d.Room)
                .Include(d => d.EnergyUsages)
                .ToListAsync();
        }

        public async Task<Device> GetDeviceWithDetailsAsync(int deviceId)
        {
            return await _context.Devices
                .Include(d => d.Room)
                .Include(d => d.House)
                .Include(d => d.EnergyUsages)
                .Include(d => d.Schedules)
                .Include(d => d.Limits)
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId);
        }
    }
}
