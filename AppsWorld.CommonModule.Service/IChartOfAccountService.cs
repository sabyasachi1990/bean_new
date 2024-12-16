using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.CommonModule.Service
{
    public interface IChartOfAccountService : IService<ChartOfAccount>
    {
        ChartOfAccount GetChartOfAccountById(long id);
        List<ChartOfAccount> GetChartOfAccounts(long AccountTypeId, long coaId);
        List<ChartOfAccount> GetAllChartOfAccountsByUsername(long CompanyId, string username);
        List<ChartOfAccount> GetCOAEdit(long id, long companyId);
        //ChartOfAccount GetChartOfAccount(string name, long companyId);
        List<ChartOfAccount> GetAllChartOfAccounts(long CompanyId);
        ChartOfAccount GetChartOfAccountByName(string name, long companyId);
        ChartOfAccount GetChartOfAccount(long id);
        ChartOfAccount GetChartOfAccountByCompanyId(long companyId);
        long GetChartOfAccountByNature(string nature, long CompanyId);
        long? GetChartOfAccountIDByName(string name, long companyId);
        List<ChartOfAccount> GetCashAndBankCOAId(long companyId, long accountid);
        List<ChartOfAccount> lstchartofaccount(long accountTypeId);
        List<COALookup<string>> listOfChartOfAccounts(long companyId, bool iSedit);
        List<COALookup<string>> ListCOADetail(long companyId, bool isEdit);
        List<ChartOfAccount> GetAllBalanceSheet(long companyId, string name);
        List<ChartOfAccount> GellAllCashByCurrency(long companyId, long? serviceCompanyId);
        List<ChartOfAccount> GetAllCOAByBank(long companyId, List<long> lstCOAId, List<long> serviceCompanyId);
        List<ChartOfAccount> GetAllCOAById(long companyId, List<long> lstCoaId);
        string GetCOAName(long coaId, long companyId);
        List<COALookup<string>> GLClearingCOAs(long companyId, bool iSedit);
        List<ChartOfAccount> GetChartOfActiveInactiveBanksAccounts(long companyId, long accountTypeId);
        List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId);
        List<ChartOfAccount> GetAllCOAByIds(List<long> lstCoaId);
        List<long> GetAllCOAByIdsAndALE(List<long> lstCoaId, string classes);
        ChartOfAccount GetWorkFolwItemByCoaId(long? companyId);
        List<long> GetAllCOAIdsByIds(List<long> lstCoaId);
        List<ChartOfAccount> GetAllCOAOfBank(long companyId, List<long> lstCOAId, long serviceCompanyId);
        List<long> GetCOAIDsByName(List<String> coaNames, long companyId);
        long? GetCoaIdByNameAndCompanyId(string name, long companyId);
        Dictionary<long, string> GetChartofAccounts(List<string> Name, long companyId);
        Dictionary<long, string> GetAllChartofAccountsById(List<long> Ids, long companyId);
        long GetICAccountId(long companyId, long serviceCompanyId);

        ChartOfAccount GetByName(string name, long companyId);
    }
}
