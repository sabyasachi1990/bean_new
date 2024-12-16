using AppsWorld.CashSalesModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public interface IMasterCompactService : IService<BeanEntityCompact>
    {
        FinancialSettingCompact GetFinancialSetting(long companyId);
        bool ValidateYearEndLockDate(DateTime DocDate, long companyId);
        DateTime? GetFinancialYearEndLockDate(long companyId);
        bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);
        bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);
        string GetEntityName(Guid? id);
        Dictionary<long, string> GetCompanyByCompanyid(long companyId);
        long? GetTaxPaybleGstCOA(long? companyId, string name);
        TaxCodeCompact GetTaxById(long taxId);
        long? GetCOAId(long? companyId, long? coaId);
    }
}
