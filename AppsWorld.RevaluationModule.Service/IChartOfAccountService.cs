using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public interface IChartOfAccountService : IService<ChartOfAccount>
    {
        List<ChartOfAccount> GetChartOfAccount(long id, long comapanyId);
        bool? GetChartOfAccountRevaluation(long id, long comapanyId);
        ChartOfAccount GetChartOfAccountName(long id, long comapanyId);

        long GetChartOfAccountId(long CompanyId, string Name);
        //List<ChartOfAccount> GetAllChartOfAccountByCid(long companyId, List<long> coaId);
        List<ChartOfAccount> GetAllChartOfAccountByCid(long companyId);
        List<ChartOfAccount> GetAllChartOfAccountByCIdAndId(List<long> coaIds, long companyId);
    }
}
