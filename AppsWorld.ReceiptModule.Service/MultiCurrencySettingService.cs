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
	public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
		private readonly IReceiptModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencySettingRepository;

		public MultiCurrencySettingService(IReceiptModuleRepositoryAsync<MultiCurrencySetting> multiCurrencySettingRepository)
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
