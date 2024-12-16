using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
	public interface IChartOfAccountService : IService<ChartOfAccount>
    {
		ChartOfAccount GetChartOfAccountById(long id);
        List<ChartOfAccount> GetCOAEdit(long id, long companyId);
        List<ChartOfAccount> GetCOANew(long companyId);
        ChartOfAccount GetByName(string name, long companyId);
        List<COALookup<string>> listOfChartOfAccounts(long companyId, bool iSedit);
        List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId);
    }
}
