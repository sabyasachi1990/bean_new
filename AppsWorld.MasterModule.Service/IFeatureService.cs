﻿using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IFeatureService:IService<Feature>
    {
        List<Guid> GetFeatureIdsByMId(long moduleId);
    }
}