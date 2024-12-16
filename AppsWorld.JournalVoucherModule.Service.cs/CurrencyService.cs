using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.CommonModule.Infra;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CurrencyService : Service<Currency>, ICurrencyService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Currency> _currencyRepository;


        public CurrencyService(IJournalVoucherModuleRepositoryAsync<Currency> currencyRepository)
			: base(currencyRepository)
        {
			_currencyRepository = currencyRepository;
        }

       public Currency GetCurrencyByCode(long companyId, string code)
	   {
		   return _currencyRepository.Query(c => c.CompanyId == companyId && c.Code == code).Select().FirstOrDefault();
	   }
	   public async Task<LookUpCategory<string>> GetByCurrencies(long CompanyId, string CategoryCode)
	   {
		   var currencyOne = await Task.Run(()=> _currencyRepository.Query(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).Select().FirstOrDefault());
		   var lookUpCurrency = new LookUpCategory<string>();

           var currencyAll = await Task.Run(()=> _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active));

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
	   public async Task<LookUpCategory<string>> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode)
	   {
		   var currencyOne = await Task.Run(()=> _currencyRepository.Query(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).Select().FirstOrDefault());
		   var lookUpCurrency = new LookUpCategory<string>();

           var currencyAll = await Task.Run(()=> _currencyRepository.Query().Select().Where(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.Code == code)));

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
