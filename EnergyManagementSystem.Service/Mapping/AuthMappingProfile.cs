using AutoMapper;
using EnergyManagementSystem.Core.DTOs.Auth;
using EnergyManagementSystem.Core.DTOs.Device;
using EnergyManagementSystem.Core.DTOs.EnergyUsage;
using EnergyManagementSystem.Core.DTOs.House;
using EnergyManagementSystem.Core.DTOs.Notification;
using EnergyManagementSystem.Core.DTOs.Room;
using EnergyManagementSystem.Core.DTOs.Schedule;
using EnergyManagementSystem.Core.DTOs.User;
using EnergyManagementSystem.Core.DTOs.UserSetting;
using EnergyManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Service.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<User, LoginDto>();
            CreateMap<User, RegisterDto>();
            CreateMap<User, TokenDto>();
            // Gerekirse tersine mapping'ler
            CreateMap<RegisterDto, User>();
        }
    }

    public class DeviceMappingProfile : Profile
    {
        public DeviceMappingProfile()
        {
            CreateMap<Device, DeviceDto>();
            CreateMap<Device, CreateDeviceDto>();
            CreateMap<CreateDeviceDto, Device>();
            CreateMap<UpdateDeviceDto, Device>();
        }
    }

    public class EnergyUsageMappingProfile : Profile
    {
        public EnergyUsageMappingProfile()
        {
            CreateMap<EnergyUsage, EnergyUsageDto>();
            CreateMap<CreateEnergyUsageDto, EnergyUsage>();
        }
    }

    public class HouseMappingProfile : Profile
    {
        public HouseMappingProfile()
        {
            CreateMap<House, HouseDto>();
            CreateMap<CreateHouseDto, House>();
            CreateMap<UpdateHouseDto, House>();
        }
    }

    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Notification>();
        }
    }

    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
        }
    }

    public class ScheduleMappingProfile : Profile
    {
        public ScheduleMappingProfile()
        {
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<CreateScheduleDto, Schedule>();
            CreateMap<UpdateScheduleDto, Schedule>();
        }
    }

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<ChangePasswordDto, User>();
        }
    }

    public class UserSettingMappingProfile : Profile
    {
        public UserSettingMappingProfile()
        {
            CreateMap<UserSetting, UserSettingDto>();
            CreateMap<CreateUserSettingDto, UserSetting>();
            CreateMap<UpdateUserSettingDto, UserSetting>();
        }
    }
}
