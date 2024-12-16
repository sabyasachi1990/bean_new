using AppsWorld.MasterModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ICommonForexService:IService<CommonForex>
    {
        Task<CommonForex> GetForexyByDateAndCurrency(long comapanyId, string fromCurrency, string toCurrency, DateTime fromDate);
    }
}
