﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class RoleListViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class AddRoleViewModel
    {
        public string RoleName { get; set; }
    }
}
