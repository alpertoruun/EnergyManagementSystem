using System.ComponentModel.DataAnnotations;

namespace EnergyManagementSystem.Core.Models
{
    public class UserSetting
    {
        [Key]
        public int SettingId { get; set; }
        public int UserId { get; set; }
        public string Preference { get; set; }  // "theme", "power_saving_mode" etc.
        public string Value { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
