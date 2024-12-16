using AppsWold.InvoiceModule.RepositoryPattern;
using AppsWorld.InvoiceModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class CommonForexService:Service<CommonForex>, ICommonForexService
    {
        private readonly InvoiceModuleRepository<CommonForex> _commonForexRepository;

        public CommonForexService(InvoiceModuleRepository<CommonForex> commonForexRepository)
            : base(commonForexRepository)
        {
            this._commonForexRepository = commonForexRepository;
        }

        public CommonForex GetForexyByDateAndCurrency(long comapanyId, string fromCurrency, string toCurrency, DateTime fromDate)
        {
            return _commonForexRepository.Query(a => a.FromCurrency == fromCurrency && a.ToCurrency == toCurrency && a.DateFrom == fromDate).Select().FirstOrDefault();
        }
    }
}
