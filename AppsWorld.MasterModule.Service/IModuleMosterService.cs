﻿using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IModuleMosterService : IService<ModuleMaster>
    {
        ModuleMaster GetModuleMaster(long companyId, string name);
    }
}