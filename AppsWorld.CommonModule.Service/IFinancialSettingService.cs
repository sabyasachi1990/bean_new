using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Models;

namespace AppsWorld.CommonModule.Service
{
    public interface IFinancialSettingService : IService<FinancialSetting>
    {
        FinancialSetting GetFinancialSetting(long companyId);
        DateTime? GetFinancialOpenPeriodStarDate(long companyId);

        DateTime? GetFinancialOpenPeriodEndDate(long companyId);

        DateTime? GetFinancialYearEndLockDate(long companyId);

        bool ValidateYearEndLockDate(DateTime DocDate, long companyId);

        bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);

        bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);
        bool RevaluationPeriodLuck(DateTime DocDate, long companyId);
        string GetFinancialSettingByCurrency(long companyId);
        decimal? GetExRateInformation(string DocumentCurrency, DateTime? Documentdate, long CompanyId);
        Task<FinancialSetting> GetFinancialSettingAsync(long companyId);

    }
}
