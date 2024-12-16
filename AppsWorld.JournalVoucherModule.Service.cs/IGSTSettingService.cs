using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
	public interface IGSTSettingService : IService<GSTSetting>
    {
		GSTSetting GetByCompanyId(long companyId);

		GSTSetting GetGSTSettings(long companyId);
		bool IsGSTDeregistered(long companyId);

		bool IsGSTSettingActivated(long companyId);
        bool IsGSTDeregisteredForSeviceCompany(long companyId);
    }
}
