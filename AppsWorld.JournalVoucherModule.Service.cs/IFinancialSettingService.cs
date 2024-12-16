using System;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
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
        Task<FinancialSetting> GetFinancialSettingAsync(long companyId);
    }
}
