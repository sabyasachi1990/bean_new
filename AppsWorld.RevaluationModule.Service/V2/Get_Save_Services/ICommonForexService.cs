using AppsWorld.RevaluationModule.Entities.Models.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Service.V2.Get_Save_Services
{
    public interface ICommonForexService : IService<CommonForex>
    {
        CommonForex GetForexyByDateAndCurrency(long comapanyId, string fromCurrency, string toCurrency, DateTime fromDate);
        Dictionary<string, decimal> GetListOfExchangeRates(long companyId, List<string> lstOfTocurrencies, string fromCurrency, DateTime fromDate);
    }
}
