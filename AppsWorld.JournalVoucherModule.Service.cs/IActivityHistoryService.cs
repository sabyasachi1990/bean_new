﻿using AppsWorld.JournalVoucherModule.Entities;
//using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
   public interface IActivityHistoryService:IService<ActivityHistory>
    {
       List<ActivityHistory> GetByCIdAId(long companyid, Guid id,string type);
    }
}