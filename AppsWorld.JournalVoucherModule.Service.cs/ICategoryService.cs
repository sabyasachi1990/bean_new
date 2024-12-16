using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ICategoryService : IService<Category>
    {
        List<SubCategory> Getsubcategory(long companyid);
        List<Category> GetCategories(long companyid);
        List<AccountType> GetAllAccounyTypeByCompanyId(long companyid);
        List<TrailBalanceSpModel> GetAllAccountsBy_Bean_HTMLTrailBalanceSP(long companyid, string compnayName, DateTime date, long frequency, int Period, int SamePeriod);

         List<IncomeStatementSpModel> GetAllAccountsBy_Bean_HTMLIncomeStatmentSP(long companyid, string compnayName, DateTime fromdate, DateTime todate, int samePeriod, int period, long frequency, bool isInterco);
        List<BalanceSheetSpModel> GetAllAccountsBy_Bean_HTMLBalanceSheetSP(long companyid, string companyName, DateTime asOfDate, int frequency, int period, int SamePeriod);
        Category GetCategorie(Guid categoryId);

        //long GetMaxIdFromCategory();
    }
}
