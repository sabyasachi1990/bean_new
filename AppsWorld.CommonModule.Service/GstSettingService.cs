using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public class GstSettingService : Service<GSTSetting>, IGSTSettingService
    {
        private readonly ICommonModuleRepositoryAsync<GSTSetting> _gstSettingRepository;

        public GstSettingService(ICommonModuleRepositoryAsync<GSTSetting> gstSettingRepository)
            : base(gstSettingRepository)
        {
            _gstSettingRepository = gstSettingRepository;
        }
        public GSTSetting GetByCompanyId(long companyId)
        {
            return _gstSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }

        public bool IsGSTDeregistered(long companyId)
        {
            GSTSetting gstSetting = _gstSettingRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            if (gstSetting != null)
                return gstSetting.IsDeregistered.Value;
            else
                return false;
        }
        public GSTSetting GetGSTSettings(long companyId)
        {
            return _gstSettingRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public bool IsGSTSettingActivated(long companyId)
        {
            return GetGSTSettings(companyId) != null;
        }
        public GSTSetting GetGSTSetting(long companyId)
        {
            return _gstSettingRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public bool IsGSTAllowed(long companyId, DateTime docDate)
        {
            GSTSetting setting = GetGSTSetting(companyId);
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
        public bool? GetgstDetail(long? serviceCompanyId)
        {
            return _gstSettingRepository.Query(e => e.ServiceCompanyId == serviceCompanyId && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault() != null;
        }

    }
}
