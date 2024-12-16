using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IInterCompanySettingService : IService<InterCompanySetting>
    {
        InterCompanySetting GetInterCompanyClearingById(Guid Id);
        InterCompanySetting GetInterCoCompanyIdAndType(long companyId, string interType);

        bool? GetIBIsActivatedOrNot(long companyId, string InterType);
    }
}
