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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithDetailsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Houses)
                    .ThenInclude(h => h.Rooms)
                .Include(u => u.Notifications)
                .Include(u => u.UserSettings)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }

}
