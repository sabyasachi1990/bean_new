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
	public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
        private readonly IBillModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencySettingRepository;

        public MultiCurrencySettingService(IBillModuleRepositoryAsync<MultiCurrencySetting> multiCurrencySettingRepository)
			: base(multiCurrencySettingRepository)
        {
			_multiCurrencySettingRepository = multiCurrencySettingRepository;
        }
		public MultiCurrencySetting GetByCompanyId(long companyId)
		{
			return _multiCurrencySettingRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
		}
        public bool? GetByFlagCompanyId(long companyId)
        {
            return _multiCurrencySettingRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select(c => c.Id).FirstOrDefault() != 0;
        }

    }
}
