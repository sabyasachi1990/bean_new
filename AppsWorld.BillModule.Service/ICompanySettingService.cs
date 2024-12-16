using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.BillModule.Models;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
    public interface ICompanySettingService : IService<CompanySetting>
    {
        bool GetModuleStatus(string moduleName,  long companyId);
        
    }
}
