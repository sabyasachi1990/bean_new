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
	public class CompanySettingService : Service<CompanySetting>, ICompanySettingService
	{
		private readonly ICommonModuleRepositoryAsync<CompanySetting> _companySettingRepository;

		public CompanySettingService(ICommonModuleRepositoryAsync<CompanySetting> companySettingRepository)
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
