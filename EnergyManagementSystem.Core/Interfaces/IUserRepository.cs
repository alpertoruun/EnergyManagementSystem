using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserWithDetailsAsync(int userId);
    }
}
