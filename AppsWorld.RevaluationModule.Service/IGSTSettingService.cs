using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
	public interface IGSTSettingService : IService<GSTSetting>
    {
		GSTSetting GetByCompanyId(long companyId);

		GSTSetting GetGSTSettings(long companyId);
		bool IsGSTDeregistered(long companyId);

		bool IsGSTSettingActivated(long companyId);
    }
}
