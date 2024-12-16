using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class GSTSettingService : Service<GSTSetting>, IGSTSettingService
    {
        private readonly IMasterModuleRepositoryAsync<GSTSetting> _gSTSettingRepository;
        public GSTSettingService(IMasterModuleRepositoryAsync<GSTSetting> gSTSettingRepository)
            : base(gSTSettingRepository)
        {
            this._gSTSettingRepository = gSTSettingRepository;
        }
        public GSTSetting GetGSTSettingByCompanyId(long CompanyId)
        {
            return _gSTSettingRepository.Query(e => e.CompanyId == CompanyId && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public GSTSetting GetGSTSettingByCIdAndId(long Id, long CompanyId)
        {
            return _gSTSettingRepository.Query(e => e.Id == Id && e.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public GSTSetting GetGSTSetting(long companyId, DateTime docDate,long serviceCompanyId)
        {
            return _gSTSettingRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active && a.ServiceCompanyId == serviceCompanyId).Select().FirstOrDefault();
        }
        public string GetGSTSettingRepo(long companyId)
        {
            return _gSTSettingRepository.Query(a => a.CompanyId == companyId).Select(c => c.GSTRepoCurrency).FirstOrDefault();
        }


        public async Task<string> GetGSTSettingRepoAsync(long companyId)
        {
            return await Task.Run(()=> _gSTSettingRepository.Query(a => a.CompanyId == companyId).Select(c => c.GSTRepoCurrency).FirstOrDefault());
        }

        public bool IsGSTAllowed(long companyId, DateTime docDate)
        {
            GSTSetting setting = GetGSTSettingByCompanyId(companyId);
            bool isGST = setting != null;
            if (isGST)
            {
                if (setting.IsDeregistered != null && setting.IsDeregistered.Value)
                {
                    isGST = docDate < setting.DeRegistration.Value;
                }
            }
            return isGST;
        }

        public bool IsGSTSettingActivated(long CompanyId)
        {
            return GetGSTSetting(CompanyId) != null;

        }

        private GSTSetting GetGSTSetting(long CompanyId)
        {
            return _gSTSettingRepository.Query(a => a.CompanyId == CompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
        }

    }
}
