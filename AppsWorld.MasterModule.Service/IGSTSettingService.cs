using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IGSTSettingService:IService<GSTSetting>
    {
        GSTSetting GetGSTSettingByCompanyId(long CompanyId);

        GSTSetting GetGSTSettingByCIdAndId(long Id, long CompanyId);
        string GetGSTSettingRepo(long companyId);
        GSTSetting GetGSTSetting(long companyId, DateTime docDate,long serviceCompanyId);
        bool IsGSTAllowed(long companyId, DateTime docDate);

        bool IsGSTSettingActivated(long CompanyId);

        Task<string> GetGSTSettingRepoAsync(long companyId);
    }
}
