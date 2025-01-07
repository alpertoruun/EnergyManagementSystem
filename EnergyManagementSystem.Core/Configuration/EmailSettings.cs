using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.Configuration
{
    // Your configuration
    public class EmailSettings
    {
        public string Host { get; set; } = "smtp.gmail.com"; 
        public int Port { get; set; } = 587;
        public bool UseTls { get; set; } = true;
        public string Username { get; set; } = "xxxx";
        public string Password { get; set; } = "xxx";
        public string ClientUrl { get; set; } = "http://localhost:5173";
        public string ApiUrl { get; set; } = "http://localhost:5286";
    }
}
