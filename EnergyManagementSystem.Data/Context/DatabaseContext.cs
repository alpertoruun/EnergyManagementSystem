using EnergyManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EnergyManagementSystem.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EnergyUsage> EnergyUsages { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Limit> Limits { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
    }
}