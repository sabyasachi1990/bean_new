using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IMultiCurrencySettingService : IService<MultiCurrencySetting>
    {
        MultiCurrencySetting Getmulticurrency(long CompanyId);
        MultiCurrencySetting GetmulticurrencyByIdandcompanyId(long MSId, long CompanyId);
        bool IsMultiCurrencyActivated(long companyId);
    }
}
