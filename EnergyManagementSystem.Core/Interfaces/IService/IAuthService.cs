using EnergyManagementSystem.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces.IService
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        Task<bool> ConfirmEmailAsync(string email, string token);

    }
}
