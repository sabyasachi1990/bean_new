using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public interface IMultiCurrencySettingService:IService<MultiCurrencySetting>
    {
        MultiCurrencySetting GetMultiCurrency(long companyId);
        bool? GetMultiCurrencyByCompanyId(long companyId);
    }
}
