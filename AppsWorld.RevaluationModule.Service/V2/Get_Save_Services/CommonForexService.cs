using AppsWorld.RevaluationModule.Entities.Models.V2;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Service.V2.Get_Save_Services
{
    public class CommonForexService : Service<CommonForex>, ICommonForexService
    {
        private readonly IRevaluationRepositoryAsync<CommonForex> _commonForexRepository;

        public CommonForexService(IRevaluationRepositoryAsync<CommonForex> commonForexRepository)
            : base(commonForexRepository)
        {
            this._commonForexRepository = commonForexRepository;
        }

        public CommonForex GetForexyByDateAndCurrency(long comapanyId, string fromCurrency, string toCurrency, DateTime fromDate)
        {
            return _commonForexRepository.Query(a => a.FromCurrency == fromCurrency && a.ToCurrency == toCurrency && a.DateFrom == fromDate).Select().FirstOrDefault();
        }

        public Dictionary<string, decimal> GetListOfExchangeRates(long companyId, List<string> lstOfTocurrencies, string fromCurrency, DateTime fromDate)
        {
            return _commonForexRepository.Query(a => a.FromCurrency == fromCurrency && lstOfTocurrencies.Contains(a.ToCurrency) && a.DateFrom == fromDate).Select(c => new { c.ToCurrency, c.FromForexRate }).ToDictionary(c => c.ToCurrency, c => c.FromForexRate.Value);
        }
    }
}
