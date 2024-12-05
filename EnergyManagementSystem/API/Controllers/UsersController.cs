using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.DTOs.User;
using System.Threading.Tasks;

namespace EnergyManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateAsync(createUserDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            updateUserDto.UserId = id;
            await _userService.UpdateAsync(id, updateUserDto);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/users/{id}/houses
        [HttpGet("{id}/houses")]
        public async Task<IActionResult> GetUserHouses(int id)
        {
            var houses = await _userService.GetUserHousesAsync(id);
            if (houses == null || !houses.Any())
            {
                return NotFound(new { message = "No houses found for this user" });
            }
            return Ok(houses);
        }
    }
}
