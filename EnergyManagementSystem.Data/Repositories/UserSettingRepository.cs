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
    public class UserSettingRepository : GenericRepository<UserSetting>, IUserSettingRepository
    {
        private readonly DatabaseContext _context;

        public UserSettingRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserSetting>> GetSettingsByUserIdAsync(int userId)
        {
            return await _context.UserSettings
                .Where(us => us.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserSetting> GetSettingByPreferenceAsync(int userId, string preference)
        {
            return await _context.UserSettings
                .FirstOrDefaultAsync(us => us.UserId == userId && us.Preference == preference);
        }
    }
}
