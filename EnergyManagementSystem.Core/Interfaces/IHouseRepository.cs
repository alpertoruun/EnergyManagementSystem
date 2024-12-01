using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IHouseRepository : IGenericRepository<House>
    {
        Task<IEnumerable<House>> GetHousesByUserIdAsync(int userId);
        Task<House> GetHouseWithDetailsAsync(int houseId);
    }
}
