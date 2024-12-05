using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IUserService 
    { Task<UserDto> GetByIdAsync(int userId);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> CreateAsync(CreateUserDto createUserDTO);
        Task UpdateAsync(int userId, UpdateUserDto updateUserDTO);
        Task DeleteAsync(int userId);
        Task<bool> ValidateCredentialsAsync(string email, string password);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<IEnumerable<HouseDto>> GetUserHousesAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllAsync(); // Yeni metod tanýmý

    }
}
