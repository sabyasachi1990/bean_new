using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ICurrencyService : IService<Currency>
    {
        LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode);
        List<Currency> GetByCurrencyById(long companyId);
        List<Currency> GetByCurrency(long companyId, string currencyCode);
        IQueryable<Currency> GetAllCurrency(long companyId);
        Currency GetById(long id);
        Currency GetByIdandCompanyId(long id, long companyId);
        Task<LookUpCategory<string>> GetByCurrenciesByEntity(string currency, long companyId);
        Task<LookUpCategory<string>> GetByCurrenciesAsync(long CompanyId, string CategoryCode);
    }
}
