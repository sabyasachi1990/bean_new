using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
	public class GstSettingService : Service<GSTSetting>, IGSTSettingService
	{
		private readonly IReceiptModuleRepositoryAsync<GSTSetting> _gstSettingRepository;

		public GstSettingService(IReceiptModuleRepositoryAsync<GSTSetting> gstSettingRepository)
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



	}
}
