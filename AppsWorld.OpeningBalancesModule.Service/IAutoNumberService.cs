﻿using AppsWorld.OpeningBalancesModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public interface IAutoNumberService : IService<AutoNumber> /*IService<BeanAutoNumber>*/
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        //BeanAutoNumber GetAutoNumber(long companyId, string entityType);
    }
}