using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class GstSettingService : Service<GSTSetting>, IGSTSettingService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<GSTSetting> _gstSettingRepository;

        public GstSettingService(IJournalVoucherModuleRepositoryAsync<GSTSetting> gstSettingRepository)
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

        public bool IsGSTDeregisteredForSeviceCompany(long companyId)
        {
            GSTSetting gstSetting = _gstSettingRepository.Query(a => a.ServiceCompanyId == companyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            if (gstSetting != null)
                return (gstSetting.IsDeregistered == null || gstSetting.IsDeregistered == false) ? true : false;
            else
                return false;
        }
    }
}
