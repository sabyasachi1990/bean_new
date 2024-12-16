using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
	public interface IMultiCurrencySettingService : IService<MultiCurrencySetting>
    {
        bool? GetByCompanyId(long companyId);

        bool? GetMultiCurrency(long companyId);
    }
}
