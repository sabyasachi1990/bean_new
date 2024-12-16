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
    public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
        private readonly ICommonModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencySettingRepository;

        public MultiCurrencySettingService(ICommonModuleRepositoryAsync<MultiCurrencySetting> multiCurrencySettingRepository)
            : base(multiCurrencySettingRepository)
        {
            _multiCurrencySettingRepository = multiCurrencySettingRepository;
        }
        public bool? GetByCompanyId(long companyId)
        {
            return _multiCurrencySettingRepository.Query(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).Select(c => c.Id).FirstOrDefault() != 0;
        }
        public bool? GetMultiCurrency(long companyId)
        {
            return _multiCurrencySettingRepository.Query(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault() != null;
        }

    }
}
