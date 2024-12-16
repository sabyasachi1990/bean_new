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
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
    public class CurrencyService : Service<Currency>, ICurrencyService
    {
        private readonly IBillModuleRepositoryAsync<Currency> _currencyRepository;


        public CurrencyService(IBillModuleRepositoryAsync<Currency> currencyRepository)
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
            var currencyOne = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).FirstOrDefault();
            var lookUpCurrency = new LookUpCategory<string>();

            var currencyAll = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active);

            if (currencyOne != null)
            {
                lookUpCurrency.CategoryName = CategoryCode;
                lookUpCurrency.DefaultValue = currencyOne.DefaultValue;
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
            var currencyOne = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).FirstOrDefault();
            var lookUpCurrency = new LookUpCategory<string>();

            var currencyAll = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.Code == code));

            if (currencyOne != null)
            {
                lookUpCurrency.CategoryName = CategoryCode;
                lookUpCurrency.DefaultValue = currencyOne.DefaultValue;
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
