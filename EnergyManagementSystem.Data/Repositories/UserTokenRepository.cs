using EnergyManagementSystem.Core.Enums;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Models;
using EnergyManagementSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Data.Repositories
{
    public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
    {
        private readonly DatabaseContext _context;

        public UserTokenRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserToken> GetValidTokenAsync(int userId, string token, TokenType tokenType)
        {
            return await _context.UserTokens
                .FirstOrDefaultAsync(t =>
                    t.UserId == userId &&
                    t.Token == token &&
                    t.TokenType == tokenType &&
                    t.ExpiryDate > DateTime.UtcNow &&
                    !t.IsUsed);
        }

        public async Task<bool> InvalidateTokensAsync(int userId, TokenType tokenType)
        {
            var tokens = await _context.UserTokens
                .Where(t => t.UserId == userId &&
                       t.TokenType == tokenType &&
                       !t.IsUsed)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsUsed = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
