using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Configuration
{
    public class JwtSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenValidityInMinutes { get; set; }
        public int RefreshTokenValidityInDays { get; set; }
    }
}
