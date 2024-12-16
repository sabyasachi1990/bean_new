using AppsWorld.JournalVoucherModule.Entities.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CommonForexService:Service<CommonForex>,ICommonForexService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CommonForex> _commonForexRepository;

        public CommonForexService(IJournalVoucherModuleRepositoryAsync<CommonForex> commonForexRepository)
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
