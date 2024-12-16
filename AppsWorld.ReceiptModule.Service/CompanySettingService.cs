using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.ReceiptModule.Service
{
	public class CompanySettingService : Service<CompanySetting>, ICompanySettingService
	{
		private readonly IReceiptModuleRepositoryAsync<CompanySetting> _companySettingRepository;

		public CompanySettingService(IReceiptModuleRepositoryAsync<CompanySetting> companySettingRepository)
			: base(companySettingRepository)
		{
			_companySettingRepository = companySettingRepository;
		}
		public bool GetModuleStatus(string moduleName, long companyId)
		{
			CompanySetting setting =  _companySettingRepository.Query(a => a.ModuleName == moduleName && a.CompanyId == companyId).Select().FirstOrDefault();

			if (setting != null)
				return setting.IsEnabled;
			return false;

		}
	}
}
