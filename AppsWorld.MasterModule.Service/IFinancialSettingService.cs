using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
   public interface IFinancialSettingService:IService<FinancialSetting>
    {
        FinancialSetting GetFinancialNyCompanyId(long CompanyId);
        string GetFinancial(long CompanyId);
        FinancialSetting GetFinancialByIdCId(long Id, long CompanyId);
        DateTime? GetFinancialYearEndLockDate(long companyId);
        bool ValidateYearEndLockDate(DateTime DocDate, long companyId);
        bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);
        FinancialSetting GetFinancialSetting(long companyId);
        bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);
        DateTime? GetFinancialOpenPeriodStarDate(long companyId);
        DateTime? GetFinancialOpenPeriodEndDate(long companyId);
        FinancialSetting VerifyFinancialLockPeriodPassword(string password, long companyId);
        bool GetFinancialByCid(long companyid);

        Task<string> GetBaseCurrencyByCompanyId(long companyId);
       
    }
}
