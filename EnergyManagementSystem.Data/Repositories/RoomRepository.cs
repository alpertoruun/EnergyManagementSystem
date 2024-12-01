using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace EnergyManagementSystem.Data.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly DatabaseContext _context;

        public RoomRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetRoomsByHouseIdAsync(int houseId)
        {
            return await _context.Rooms
                .Where(r => r.HouseId == houseId)
                .Include(r => r.Devices)
                .ToListAsync();
        }

        public async Task<Room> GetRoomWithDevicesAsync(int roomId)
        {
            return await _context.Rooms
                .Include(r => r.Devices)
                    .ThenInclude(d => d.EnergyUsages)
                .Include(r => r.House)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }
    }
}
