using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.DTOs.User;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            var hashedPassword = _passwordHasher.HashPassword(createUserDto.Password);

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                Password = hashedPassword,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);
            return new UserDto
            {
                UserId = createdUser.UserId,
                Name = createdUser.Name,
                Email = createdUser.Email
            };
        }

        public async Task UpdateAsync(int userId, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            await _userRepository.DeleteAsync(user);
        }
        public async Task<IEnumerable<HouseDto>> GetUserHousesAsync(int userId)
        {
            var user = await _userRepository.GetUserWithDetailsAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            return user.Houses.Select(house => new HouseDto
            {
                HouseId = house.HouseId,
                Name = house.Name,
                Address = house.Address,
                PowerSavingMode = house.PowerSavingMode,
                UserId = house.UserId
            });
        }

        public async Task<bool> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            return _passwordHasher.VerifyPassword(password, user.Password);
        }
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            });
        }

        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!_passwordHasher.VerifyPassword(currentPassword, user.Password))
                throw new UnauthorizedAccessException("Invalid current password");

            user.Password = _passwordHasher.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
        }
    }
}
