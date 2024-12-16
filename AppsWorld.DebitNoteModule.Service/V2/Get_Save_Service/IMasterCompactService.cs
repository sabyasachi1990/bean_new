using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Entities.V2;
using Service.Pattern;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public interface IMasterCompactService : IService<BeanEntityCompact>
    {
        decimal? GetCteditLimitsValue(Guid id);
        string GetEntityName(Guid? id);
        FinancialSettingCompact GetFinancialSetting(long companyId);
        bool ValidateYearEndLockDate(DateTime DocDate, long companyId);
        DateTime? GetFinancialYearEndLockDate(long companyId);
        bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);
        bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);
        CompanyCompact GetCompanyByCompanyid(long companyId);
        long GetChartOfAccountByNature(string nature, long CompanyId);
        string GetIdBy(long Id);
        long? GetTaxPaybleGstCOA(long? companyId, string name);
        List<TaxCodeCompact> GetTaxCodes(long companyId);
        TaxCodeCompact GetTaxById(long taxId);
        List<TaxCodeCompact> GetAllTaxById(List<long?> taxId);
        Dictionary<long, string> GetTaxCodes(List<long?> taxIds, long companyId);
        List<LookUpCompany<string>> GetSubCompany(string username, long companyId, long? subcompanyid);
        List<CompanyCompact> GetCompany(long companyId, long? companyIdCheck, string username = null);
        List<TaxCodeCompact> GetTaxAllCodes(long companyId, DateTime? date);
        ChartOfAccountCompact GetChartOfAccountByName(string name, long companyId);
        IDictionary<long, string> GetById(long id);
        Dictionary<string, decimal?> GetEntityDataById(Guid entityId);
        IDictionary<double?, string> GetTermsOfPayment(long? id);
    }
}
