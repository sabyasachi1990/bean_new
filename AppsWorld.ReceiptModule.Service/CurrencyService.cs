using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
    public class CurrencyService : Service<Currency>, ICurrencyService
    {
        private readonly IReceiptModuleRepositoryAsync<Currency> _currencyRepository;

        public CurrencyService(IReceiptModuleRepositoryAsync<Currency> currencyRepository)
            : base(currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public Currency GetCurrencyByCode(long companyId, string code)
        {
            return _currencyRepository.Query(c => c.CompanyId == companyId && c.Code == code).Select().FirstOrDefault();
        }
        public  LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode)
        {
           
            LookUpCategory<string> lookUpCurrency = new LookUpCategory<string>();
            List<Currency> currencyAll =  _currencyRepository.Queryable().Where(a => a.CompanyId == CompanyId &&( a.Status == AppsWorld.Framework.RecordStatusEnum.Active|| a.DefaultValue == CategoryCode)).ToList();

            if (currencyAll.Any(c=>c.DefaultValue==CategoryCode))
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
           
            LookUpCategory<string> lookUpCurrency = new LookUpCategory<string>();
            List<Currency> currencyAll =  _currencyRepository.Queryable().Where(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.Code == code || a.DefaultValue == CategoryCode)).ToList();

            if (currencyAll.Exists(c => c.DefaultValue == CategoryCode))
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
