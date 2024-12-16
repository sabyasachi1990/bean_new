using AppsWorld.MasterModule.Entities.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CommonForexService:Service<CommonForex>,ICommonForexService
    {
        private readonly IMasterModuleRepositoryAsync<CommonForex> _commonForexRepository;
       
        public CommonForexService(IMasterModuleRepositoryAsync<CommonForex> commonForexRepository)
            : base(commonForexRepository)
        {
            this._commonForexRepository = commonForexRepository;
        }

        public async Task<CommonForex> GetForexyByDateAndCurrency(long comapanyId, string fromCurrency, string toCurrency, DateTime fromDate)
        {
            return await Task.Run(()=> _commonForexRepository.Query(a => a.FromCurrency == fromCurrency && a.ToCurrency == toCurrency && a.DateFrom == fromDate).Select().FirstOrDefault());
        }
    }
}
