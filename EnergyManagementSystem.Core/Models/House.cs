namespace EnergyManagementSystem.Core.Models
{
    public class House
    {
        public int HouseId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool PowerSavingMode { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}
