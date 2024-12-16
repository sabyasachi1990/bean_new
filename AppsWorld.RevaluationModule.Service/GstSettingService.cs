using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.RevaluationModule.Entities;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public class GstSettingService : Service<GSTSetting>, IGSTSettingService
    {
        private readonly IRevaluationModuleRepositoryAsync<GSTSetting> _gstSettingRepository;

        public GstSettingService(IRevaluationModuleRepositoryAsync<GSTSetting> gstSettingRepository)
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

		
    }
}
