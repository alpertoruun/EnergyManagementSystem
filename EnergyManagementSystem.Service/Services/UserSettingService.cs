using EnergyManagementSystem.Core.DTOs.UserSetting;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using AutoMapper;

namespace EnergyManagementSystem.Service.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IUserSettingRepository _userSettingRepository;
        private readonly IMapper _mapper;

        public UserSettingsService(IUserSettingRepository userSettingRepository, IMapper mapper)
        {
            _userSettingRepository = userSettingRepository;
            _mapper = mapper;
        }

        public async Task<UserSettingDto> GetAllSettingsAsync(int userId)
        {
            var settings = await _userSettingRepository.GetSettingsByUserIdAsync(userId);
            return _mapper.Map<UserSettingDto>(settings.FirstOrDefault());
        }

        public async Task<string> GetPreferenceAsync(int userId, string key)
        {
            var setting = await _userSettingRepository.GetSettingByPreferenceAsync(userId, key);
            return setting?.Value;
        }

        public async Task UpdateSettingAsync(int userId, string key, string value)
        {
            var setting = await _userSettingRepository.GetSettingByPreferenceAsync(userId, key);

            if (setting == null)
            {
                setting = new UserSetting
                {
                    UserId = userId,
                    Preference = key,
                    Value = value
                };
                await _userSettingRepository.AddAsync(setting);
            }
            else
            {
                setting.Value = value;
                await _userSettingRepository.UpdateAsync(setting);
            }
        }

        public async Task UpdateSettingsAsync(int userId, UpdateUserSettingDto settings)
        {
            var userSetting = await _userSettingRepository.GetSettingByPreferenceAsync(userId, settings.Preference);

            if (userSetting == null)
            {
                userSetting = _mapper.Map<UserSetting>(settings);
                userSetting.UserId = userId;
                await _userSettingRepository.AddAsync(userSetting);
            }
            else
            {
                _mapper.Map(settings, userSetting);
                await _userSettingRepository.UpdateAsync(userSetting);
            }
        }
    }
}