using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;

namespace AppsWorld.BillModule.Service
{
    public class GstSettingService : Service<GSTSetting>, IGSTSettingService
    {
        private readonly IBillModuleRepositoryAsync<GSTSetting> _gstSettingRepository;

        public GstSettingService(IBillModuleRepositoryAsync<GSTSetting> gstSettingRepository)
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
        public bool IsGSTSettingActivateByServiceCompany(long companyId, DateTime docDate, long serviceCompanyId)
        {
            //GstModel gst = new GstModel();
            GSTSetting setting = _gstSettingRepository.Query(a => a.CompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active && a.ServiceCompanyId == serviceCompanyId).Select().FirstOrDefault();
            bool isGST = setting != null;
            if (isGST)
            {
                if (setting.IsDeregistered != null && setting.IsDeregistered.Value)
                {
                    isGST = docDate < setting.DeRegistration.Value;
                }
                //gst.IsGstActive = isGST;
                //gst.GstExCurrency = setting.GSTRepoCurrency;
            }
            return isGST;
        }
        public string GetGSTByServiceCompany(long servicCcompanyId)
        {
            return _gstSettingRepository.Query(a => a.ServiceCompanyId == servicCcompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select(c => c.GSTRepoCurrency).FirstOrDefault();
        }
    }
}
