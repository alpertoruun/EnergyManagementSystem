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
    public class HouseRepository : GenericRepository<House>, IHouseRepository
    {
        private readonly DatabaseContext _context;

        public HouseRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<House>> GetHousesByUserIdAsync(int userId)
        {
            return await _context.Houses
                .Where(h => h.UserId == userId)
                .Include(h => h.Rooms)
                .ToListAsync();
        }

        public async Task<House> GetHouseWithDetailsAsync(int houseId)
        {
            return await _context.Houses
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.Devices)
                .Include(h => h.Devices)
                .FirstOrDefaultAsync(h => h.HouseId == houseId);
        }
    }
}
