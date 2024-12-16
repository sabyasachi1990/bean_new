using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CompanySettingService : Service<CompanySetting>, ICompanySettingService
	{
        private readonly IJournalVoucherModuleRepositoryAsync<CompanySetting> _companySettingRepository;

        public CompanySettingService(IJournalVoucherModuleRepositoryAsync<CompanySetting> companySettingRepository)
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
