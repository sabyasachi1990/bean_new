using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IChartOfAccountService : IService<ChartOfAccount>
    {
        ChartOfAccount GetChartOfAccountById(long id);
        ChartOfAccount GetCOAEdit(long id, long companyId);
        List<ChartOfAccount> GetCOANew(long companyId);
        ChartOfAccount GetByName(string name, long companyId);
        List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId);
        List<ChartOfAccount> GetChartofAccountsByCompanyId(long companyId);
        List<ChartOfAccount> GetCOAbycategoryid(Guid categoryId);
        ChartOfAccount GetByIdLid(Guid id, Guid value);
        ChartOfAccount GetById(Guid id);
        Dictionary<long, string> GetChartofAccounts(List<long> coaIds, long companyId);
        long GetByNameAndCompanyId(string name, long companyId);

        //ChartOfAccount based on accType and serviceEntityIds
        Dictionary<long, long?> GetListChartofAccounts(List<long> accTypeIds, long companyId, List<long> servEntityIds);

    }
}
