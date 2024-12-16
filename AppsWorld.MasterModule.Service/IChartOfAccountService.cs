using AppsWorld.CommonModule.Infra;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IChartOfAccountService : IService<ChartOfAccount>
    {
        List<ChartOfAccount> GetChartOfAccountByDisable(long CompanyId);

        List<ChartOfAccount> GetChartOfAccountByRevaluation(long CompanyId);

        IQueryable<ChartOfAccount> GetChartOfAccountBycid(long CompanyId);

        List<ChartOfAccount> GetChartOfAccountByIdcid(long chartofaccountid, long CompanyId);

        List<ChartOfAccount> GetChartOfAccountBycidcode(string code, long CompanyId);

        List<ChartOfAccount> GetChartOfAccountBycidIdCode(long coaid, string code, long CompanyId);

        List<COALookup<string>> GetChartOfAccountBycidAndId(long coa, long CompanyId);

        List<COALookup<string>> GetChartOfAccountByCId(long CompanyId);

        ChartOfAccount GetChartOfAccountById(long id);
        ChartOfAccount CheckName(long id, string name, long companyId);
        List<ChartOfAccount> CheckCode(string code, long companyId);
        List<long> GetChartOfAccountId(long companyId);
        string GetCOAName(long coaId, long companyId);
        List<ChartOfAccount> GetChartOfAccountByAccountTypeCOA(List<long> accountTypeId);
        List<ChartOfAccount> GetChartOfAccountByAccountType(long accountTypeId);
        ChartOfAccount GetOBChartOfdAccount(long companyId);
        Task<IQueryable<ChartOfAccountModelK>>  GetAllCOAK(long companyId,string username);
        ChartOfAccount GetChartOfAccount(long companyId, long coaId);
        Task<List<ChartOfAccount>> GetChartOfAccountByAccountTypeCOAAsync(List<long> accountTypeId);
    }
}
