using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class MultiCurrencySettingService : Service<MultiCurrencySetting>, IMultiCurrencySettingService
    {
        private readonly IMasterModuleRepositoryAsync<MultiCurrencySetting> _multiCurrencySettingRepository;
        public MultiCurrencySettingService(IMasterModuleRepositoryAsync<MultiCurrencySetting> multiCurrencySettingRepository)
            : base(multiCurrencySettingRepository)
        {
            _multiCurrencySettingRepository = multiCurrencySettingRepository;
        }
        public MultiCurrencySetting Getmulticurrency(long CompanyId)
        {
            return _multiCurrencySettingRepository.Query(x => x.CompanyId == CompanyId && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public MultiCurrencySetting GetmulticurrencyByIdandcompanyId(long MSId, long CompanyId)
        {
            return _multiCurrencySettingRepository.Query(e => e.Id == MSId && e.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public bool IsMultiCurrencyActivated(long companyId)
        { 
            return _multiCurrencySettingRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault() != null;
        }

    }
}
