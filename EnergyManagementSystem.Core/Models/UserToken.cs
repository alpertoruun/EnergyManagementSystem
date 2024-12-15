using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergyManagementSystem.Core.Enums;

namespace EnergyManagementSystem.Core.Models
{
    public class UserToken
    {
        [Key]
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public TokenType TokenType { get; set; } // For EMAIL_CONFIRMATION, PASSWORD_RESET, etc.
        public string? Data { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
