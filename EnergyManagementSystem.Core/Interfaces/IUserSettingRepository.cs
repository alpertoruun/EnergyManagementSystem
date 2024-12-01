using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IUserSettingRepository : IGenericRepository<UserSetting>
    {
        Task<IEnumerable<UserSetting>> GetSettingsByUserIdAsync(int userId);
        Task<UserSetting> GetSettingByPreferenceAsync(int userId, string preference);
    }
}
