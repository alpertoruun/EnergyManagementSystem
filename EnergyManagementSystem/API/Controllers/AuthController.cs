using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.DTOs.Auth;
using EnergyManagementSystem.Core.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using EnergyManagementSystem.Core.DTOs.User;
using System.Security.Claims;

namespace EnergyManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            return Ok(result);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _authService.ConfirmEmailAsync(email, token);

            if (result)
            {
                return Ok(new
                {
                    success = true,
                    message = "Email confirmed successfully. You can now login."
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Invalid token or email."
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _authService.ForgotPasswordAsync(email);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string email, string token, [FromBody] string newPassword)
        {
            var result = await _authService.ResetPasswordAsync(email, token, newPassword);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await _authService.RevokeRefreshTokenAsync(refreshToken);
            return NoContent();
        }
    }
}