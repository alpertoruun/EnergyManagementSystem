﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Core.DTOs.UserSetting
{
    public class CreateUserSettingDto
    {
        public int UserId { get; set; }
        public string Preference { get; set; }
        public string Value { get; set; }
    }

}
