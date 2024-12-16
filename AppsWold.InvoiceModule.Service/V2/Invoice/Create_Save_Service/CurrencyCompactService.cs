using Service.Pattern;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.CommonModule.Infra;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class CurrencyCompactService : Service<CurrencyCompact>, ICurrencyCompactService
    {
        private readonly IInvoiceComptModuleRepositoryAsync<CurrencyCompact> _currencyRepository;

        public CurrencyCompactService(IInvoiceComptModuleRepositoryAsync<CurrencyCompact> currencyRepository)
            : base(currencyRepository)
        {
            this._currencyRepository = currencyRepository;
        }
        public LookUpCategory<string> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode)
        {
            //var currencyOne = _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).FirstOrDefault();
            var lookUpCurrency = new LookUpCategory<string>();

            var currencyAll = _currencyRepository.Queryable().Where(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.Code == code || a.DefaultValue == CategoryCode));

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
    }
}

