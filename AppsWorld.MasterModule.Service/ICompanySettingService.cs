using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
   public interface ICompanySettingService:IService<CompanySetting>
    {
        CompanySetting ActivateModule(string moduleName, long CompanyId);

        CompanySetting ActivateModuleM(string moduleName, long companyId);

        bool GetModuleStatuss(string moduleName, long companyId);
        bool ActivateModules(string moduleName, long companyId);
    }
}
