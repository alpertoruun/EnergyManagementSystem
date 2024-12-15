using EnergyManagementSystem.Core.Enums;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Interfaces
{
    public interface IUserTokenRepository : IGenericRepository<UserToken>
    {
        Task<UserToken> GetValidTokenAsync(int userId, string token, TokenType tokenType);
        Task<bool> InvalidateTokensAsync(int userId, TokenType tokenType);
        Task<UserToken> GetTokenByValueAsync(string token, TokenType tokenType);
    }
}
