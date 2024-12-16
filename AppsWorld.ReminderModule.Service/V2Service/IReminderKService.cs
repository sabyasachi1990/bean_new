using AppsWorld.ReminderModule.Entities.V2Entities;
using AppsWorld.ReminderModule.Models.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.Service.V2Service
{
    public interface IReminderKService : IService<SOAReminderBatchListEntity>
    {
        Task<IQueryable<ReminderVMK>> GetReminderskNew(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name);
    }
}
