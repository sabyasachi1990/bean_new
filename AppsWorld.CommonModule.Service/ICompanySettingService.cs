using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
    public interface ICompanySettingService : IService<CompanySetting>
    {
        bool GetModuleStatus(string moduleName,  long companyId);
        
    }
}
