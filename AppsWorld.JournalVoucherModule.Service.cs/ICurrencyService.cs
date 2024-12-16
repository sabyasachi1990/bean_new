using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
	public interface ICurrencyService : IService<Currency>
    {
		Currency GetCurrencyByCode(long companyId, string code);

		Task<LookUpCategory<string>> GetByCurrencies(long CompanyId, string CategoryCode);
		Task<LookUpCategory<string>> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode);

    }
}
