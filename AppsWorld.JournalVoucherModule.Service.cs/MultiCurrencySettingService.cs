using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencySettingRepository;

        public MultiCurrencySettingService(IJournalVoucherModuleRepositoryAsync<MultiCurrencySetting> multiCurrencySettingRepository)
			: base(multiCurrencySettingRepository)
        {
			_multiCurrencySettingRepository = multiCurrencySettingRepository;
        }
		public MultiCurrencySetting GetByCompanyId(long companyId)
		{
			return _multiCurrencySettingRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
		}

	
    }
}
