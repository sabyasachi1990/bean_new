using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal;
using AppsWorld.JournalVoucherModule.Models.V3;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service.cs.V3.Journal
{
    public interface IJournalServiceV3 : IService<CategoryV3>
    {
        List<CategoryV3> GetCategories(long companyid);

        List<SubCategoryV3> Getsubcategory(long companyid);
       

        List<AccountTypeV3> GetAllAccounyTypeByCompanyId(long companyid);

        List<IncomeStatementSpModel> GetAllAccountsBy_Bean_HTMLIncomeStatmentSP(long companyid, string compnayName, DateTime fromdate, DateTime todate, int samePeriod, int period, long frequency, bool isInterco);

        List<BalanceSheetSpModel> GetAllAccountsBy_Bean_HTMLBalanceSheetSP(long companyid, string companyName, DateTime asOfDate, int frequency, int period, int SamePeriod);

        Dictionary<long, long?> GetListChartofAccounts(List<long> accTypeIds, long companyId, List<long> servEntityIds);
    }
}
