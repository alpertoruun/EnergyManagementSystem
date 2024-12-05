namespace EnergyManagementSystem.Core.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsEmailConfirmed { get; set; }  // an Extra for mail confirmed
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<House> Houses { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserSetting> UserSettings { get; set; }
        public ICollection<UserToken> UserTokens { get; set; }
    }
}
