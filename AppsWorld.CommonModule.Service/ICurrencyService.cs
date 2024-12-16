using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.CommonModule.Service
{
	public interface ICurrencyService : IService<Currency>
    {
		Currency GetCurrencyByCode(long companyId, string code);

		LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode);
		LookUpCategory<string> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode);

    }
}
