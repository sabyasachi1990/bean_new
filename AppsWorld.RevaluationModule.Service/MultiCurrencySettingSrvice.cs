using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
        private readonly IRevaluationModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencyRepository;
        public MultiCurrencySettingService(IRevaluationModuleRepositoryAsync<MultiCurrencySetting> multiCurrencyRepository)
            : base(multiCurrencyRepository)
        {
            _multiCurrencyRepository = multiCurrencyRepository;
        }
        public MultiCurrencySetting GetMultiCurrency(long companyId)
        {
            return _multiCurrencyRepository.Query(x => x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public bool? GetMultiCurrencyByCompanyId(long companyId)
        {
            return _multiCurrencyRepository.Query(x => x.CompanyId == companyId && x.Status == RecordStatusEnum.Active).Select(c => c.Revaluation).FirstOrDefault();
        }
    }
}
