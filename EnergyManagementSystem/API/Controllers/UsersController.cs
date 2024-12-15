using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.DTOs.User;
using EnergyManagementSystem.Core.DTOs.House;
using System.Security.Claims;
using EnergyManagementSystem.Data.Repositories;
using System.Net;


namespace EnergyManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHouseService _houseService;  //DI

        public UsersController(IUserService userService, IHouseService houseService)
        {
            _userService = userService;
            _houseService = houseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                await _userService.UpdateAsync(id, updateUserDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name
            });
        }
        [Authorize]
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto model)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (currentUserId != id)
                {
                    return Forbid();
                }

                await _userService.ChangePasswordAsync(id, model.CurrentPassword, model.NewPassword);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return BadRequest(new { message = "Current password is incorrect" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/houses")]
        public async Task<ActionResult<IEnumerable<HouseDto>>> GetUserHouses(int id)
        {
            var houses = await _houseService.GetUserHousesAsync(id);
            return Ok(houses);
        }

        [Authorize] 
        [HttpPost("{id}/change-email")]
        public async Task<IActionResult> ChangeEmail(int id, [FromBody] string newEmail)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                if (currentUserId != id)
                {
                    return Forbid();
                }

                await _userService.InitiateEmailChangeAsync(id, newEmail);
                return Ok(new { message = "Confirmation email has been sent to new address." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("confirm-email-change")]
        public async Task<IActionResult> ConfirmEmailChange([FromQuery] string token, [FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                {
                    return BadRequest("Token or email is missing");
                }

                var decodedToken = WebUtility.UrlDecode(token);
                await _userService.ConfirmEmailChangeAsync(email, decodedToken);
                return Ok(new { message = "Email change confirmed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    exceptionDetails = ex.ToString()  
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "An error occurred while confirming email change",
                    exceptionDetails = ex.ToString()
                });
            }
        }
    }
}