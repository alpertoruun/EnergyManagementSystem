namespace EnergyManagementSystem.Core.Models
{
    public class Limit
    {
        public int LimitId { get; set; }
        public int DeviceId { get; set; }
        public string LimitType { get; set; }  // "kwh" or "tl"
        public decimal LimitValue { get; set; }
        public string Period { get; set; }  // "daily", "monthly" etc.

        // Navigation property
        public Device Device { get; set; }
    }
}
