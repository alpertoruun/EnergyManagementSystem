using EnergyManagementSystem.Core.DTOs.Auth;
using EnergyManagementSystem.Core.Configuration;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnergyManagementSystem.Core.Enums;
using EnergyManagementSystem.Core.Interfaces.IService;
using System.Security.Cryptography;

namespace EnergyManagementSystem.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUserRepository userRepository,
            IUserTokenRepository userTokenRepository,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!user.IsEmailConfirmed)
                throw new UnauthorizedAccessException("Please confirm your email address before logging in.");

            if (!_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid password.");

            return await GenerateTokensAsync(user);
        }


        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email is already registered.");

            var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = hashedPassword,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            var token = new UserToken
            {
                UserId = user.UserId,
                Token = GenerateRandomToken(),
                TokenType = TokenType.EmailConfirmation,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddAsync(token);
            await _emailService.SendEmailConfirmationAsync(user.Email, token.Token);

            return null; // Email onayı olmadan token dönmüyoruz
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return false;

            var validToken = await _userTokenRepository.GetValidTokenAsync(
                user.UserId,
                token,
                TokenType.EmailConfirmation);

            if (validToken == null)
                return false;

            user.IsEmailConfirmed = true;
            validToken.IsUsed = true;

            await _userRepository.UpdateAsync(user);
            await _userTokenRepository.UpdateAsync(validToken);

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return false;

            await _userTokenRepository.InvalidateTokensAsync(user.UserId, TokenType.PasswordReset);

            var token = new UserToken
            {
                UserId = user.UserId,
                Token = GenerateRandomToken(),
                TokenType = TokenType.PasswordReset,
                ExpiryDate = DateTime.UtcNow.AddHours(1),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddAsync(token);
            await _emailService.SendPasswordResetEmailAsync(user.Email, token.Token);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return false;

            var validToken = await _userTokenRepository.GetValidTokenAsync(
                user.UserId,
                token,
                TokenType.PasswordReset);

            if (validToken == null)
                return false;

            var hashedPassword = _passwordHasher.HashPassword(newPassword);
            user.Password = hashedPassword;
            validToken.IsUsed = true;

            await _userRepository.UpdateAsync(user);
            await _userTokenRepository.UpdateAsync(validToken);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (!_passwordHasher.VerifyPassword(currentPassword, user.Password))
                return false;

            var hashedPassword = _passwordHasher.HashPassword(newPassword);
            user.Password = hashedPassword;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        private async Task<TokenDto> GenerateTokensAsync(User user)
        {
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // Refresh token oluştur
            var refreshToken = new UserToken
            {
                UserId = user.UserId,
                Token = GenerateRandomToken(),
                TokenType = TokenType.RefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userTokenRepository.AddAsync(refreshToken);

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                Expiration = token.ValidTo
            };
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

        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var userToken = await _userTokenRepository.SingleOrDefaultAsync(t =>
                t.Token == refreshToken &&
                t.TokenType == TokenType.RefreshToken);

            if (userToken == null || userToken.IsUsed || userToken.ExpiryDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var user = await _userRepository.GetByIdAsync(userToken.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            // Eski refresh token'ı geçersiz kıl
            userToken.IsUsed = true;
            await _userTokenRepository.UpdateAsync(userToken);

            // Yeni access token ve refresh token üret
            return await GenerateTokensAsync(user);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var userToken = await _userTokenRepository.SingleOrDefaultAsync(t =>
                t.Token == refreshToken &&
                t.TokenType == TokenType.RefreshToken);

            if (userToken == null)
                return false;

            userToken.IsUsed = true;
            await _userTokenRepository.UpdateAsync(userToken);
            return true;
        }
    }
}