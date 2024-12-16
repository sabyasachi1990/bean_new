using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.BillModule.Models;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface IFinancialSettingService : IService<FinancialSetting>
    {
		FinancialSetting GetFinancialSetting(long companyId);
		DateTime? GetFinancialOpenPeriodStarDate(long companyId);

		DateTime? GetFinancialOpenPeriodEndDate(long companyId);

		DateTime? GetFinancialYearEndLockDate(long companyId);

        bool ValidateFinancialOpenPeriod(DateTime PostDate, long companyId);

        bool ValidateFinancialLockPeriodPassword(DateTime PostDate, string Password, long companyId);

        bool ValidateYearEndLockDate(DateTime PostDate, long companyId);
        Tuple<DateTime?, DateTime?,string> GetFinancialSettingNew(long companyId);
    }
}
