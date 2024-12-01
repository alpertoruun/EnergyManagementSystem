using EnergyManagementSystem.Core.DTOs.UserSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IUserSettingsService 
    { Task<UserSettingDto> GetAllSettingsAsync(int userId);
        Task<string> GetPreferenceAsync(int userId, string key);
        Task UpdateSettingAsync(int userId, string key, string value);
        Task UpdateSettingsAsync(int userId, UpdateUserSettingDto settings);
        
    }
}
