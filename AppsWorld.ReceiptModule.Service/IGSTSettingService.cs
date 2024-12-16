using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IGSTSettingService : IService<GSTSetting>
    {
		GSTSetting GetByCompanyId(long companyId);

		GSTSetting GetGSTSettings(long companyId);
		bool IsGSTDeregistered(long companyId);

		bool IsGSTSettingActivated(long companyId);
    }
}
