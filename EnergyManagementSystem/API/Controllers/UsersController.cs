using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.DTOs.User;
using EnergyManagementSystem.Core.DTOs.House;


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

        [HttpGet("{id}/houses")]
        public async Task<ActionResult<IEnumerable<HouseDto>>> GetUserHouses(int id)
        {
            var houses = await _houseService.GetUserHousesAsync(id);
            return Ok(houses);
        }
    }
}