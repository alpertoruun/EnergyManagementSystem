namespace EnergyManagementSystem.Core.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int HouseId { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public House House { get; set; }
        public ICollection<Device> Devices { get; set; }
    }
}
