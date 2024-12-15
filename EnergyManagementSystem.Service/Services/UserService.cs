using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.DTOs.User;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EnergyManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IEmailService _emailService;



        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IUserTokenRepository userTokenRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userTokenRepository = userTokenRepository;
            _emailService = emailService;
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
        private string GenerateRandomToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            string base64 = Convert.ToBase64String(randomBytes);

            string base64Url = base64
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
            return base64Url;
        }

        public async Task InitiateEmailChangeAsync(int userId, string newEmail)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!IsValidEmail(newEmail))
                throw new ArgumentException("Invalid email format");

            var existingUser = await _userRepository.GetUserByEmailAsync(newEmail);
            if (existingUser != null)
                throw new InvalidOperationException("This email is already in use");

            await _userTokenRepository.InvalidateTokensAsync(userId, TokenType.EmailChange);

            var token = GenerateRandomToken();
            var userToken = new UserToken
            {
                UserId = userId,
                Token = token,
                TokenType = TokenType.EmailChange,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                Data= newEmail,
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddAsync(userToken);

            await _emailService.SendEmailChangeConfirmationAsync(newEmail, token);
        }
        public async Task ConfirmEmailChangeAsync(string email, string token)
        {
            var userToken = await _userTokenRepository.GetTokenByValueAsync(token, TokenType.EmailChange);
            if (userToken == null)
            {
                throw new InvalidOperationException("Token is expired or invalid");
            }

            if (userToken.Data != email)
            {
                throw new InvalidOperationException("Token does not match with email");
            }

            var user = await _userRepository.GetByIdAsync(userToken.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.Email = email;
            user.IsEmailConfirmed = true;

            await _userTokenRepository.InvalidateTokensAsync(userToken.UserId, TokenType.EmailChange);

            await _userRepository.UpdateAsync(user);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
