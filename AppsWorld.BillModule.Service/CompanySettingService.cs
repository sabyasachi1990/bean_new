using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.BillModule.Service
{
	public class CompanySettingService : Service<CompanySetting>, ICompanySettingService
	{
        private readonly IBillModuleRepositoryAsync<CompanySetting> _companySettingRepository;

        public CompanySettingService(IBillModuleRepositoryAsync<CompanySetting> companySettingRepository)
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
