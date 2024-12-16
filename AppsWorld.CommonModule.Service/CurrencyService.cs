using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.RepositoryPattern;

namespace AppsWorld.CommonModule.Service
{
    public class CurrencyService : Service<Currency>, ICurrencyService
    {
        private readonly ICommonModuleRepositoryAsync<Currency> _currencyRepository;

        public CurrencyService(ICommonModuleRepositoryAsync<Currency> currencyRepository)
            : base(currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public Currency GetCurrencyByCode(long companyId, string code)
        {
            return _currencyRepository.Query(c => c.CompanyId == companyId && c.Code == code).Select().FirstOrDefault();
        }
        public LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode)
        {
            //var currencyOne = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).FirstOrDefault();
            var lookUpCurrency = new LookUpCategory<string>();

            var currencyAll = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.DefaultValue == CategoryCode));

            if (currencyAll.Any(c => c.DefaultValue == CategoryCode))
            {
                lookUpCurrency.CategoryName = CategoryCode;
                lookUpCurrency.DefaultValue = CategoryCode;
                lookUpCurrency.Lookups = currencyAll.Select(x => new LookUp<string>()
                {
                    Code = x.Code,
                    Name = x.Name,
                    RecOrder = x.RecOrder
                }).ToList();
            }
            return lookUpCurrency;
        }
        public LookUpCategory<string> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode)
        {
            //var currencyOne = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).FirstOrDefault();
            var lookUpCurrency = new LookUpCategory<string>();

            var currencyAll = _currencyRepository.Query(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.Code == code || a.DefaultValue == CategoryCode)).Select();

            if (currencyAll.Any(c => c.DefaultValue == CategoryCode))
            {
                lookUpCurrency.CategoryName = CategoryCode;
                lookUpCurrency.DefaultValue = CategoryCode;
                lookUpCurrency.Lookups = currencyAll.Select(x => new LookUp<string>()
                {
                    Code = x.Code,
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder
                }).ToList();
            }
            return lookUpCurrency;
        }

    }
}
