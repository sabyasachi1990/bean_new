using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CommonModule.Infra;
using AppsWorld.RevaluationModule.Entities.V2;
using Service.Pattern;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public interface IMasterCompactService : IService<BeanEntityCompact>
    {
        FinancialSettingCompact GetFinancialSetting(long companyId);
        bool ValidateYearEndLockDate(DateTime DocDate, long companyId);
        DateTime? GetFinancialYearEndLockDate(long companyId);
        bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);
        bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);
        string GetNameById(long Id);
        Dictionary<long, string> GetCOAByName(long? companyId, string name);
        List<ChartOfAccountCompact> GetAllRevalAccount(long companyId);
        bool IsMultiCurrecySettings(long companyId);
        bool RevaluationPeriodLuck(DateTime DocDate, long companyId);
        string GetBaseCurrency(long companyId);
    }
}
