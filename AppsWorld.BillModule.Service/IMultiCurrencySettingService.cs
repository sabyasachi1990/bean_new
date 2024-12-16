using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface IMultiCurrencySettingService : IService<MultiCurrencySetting>
    {
		MultiCurrencySetting GetByCompanyId(long companyId);

        bool? GetByFlagCompanyId(long companyId);



    }
}
