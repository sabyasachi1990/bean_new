using System;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
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
    }
}
