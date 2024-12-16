using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
	public interface IGSTSettingService : IService<GSTSetting>
    {
		GSTSetting GetByCompanyId(long companyId);

		GSTSetting GetGSTSettings(long companyId);
		bool IsGSTDeregistered(long companyId);

		bool IsGSTSettingActivated(long companyId);

        bool IsGSTAllowed(long companyId, DateTime docDate);

        bool? GetgstDetail(long? serviceCompanyId);
    }
}
