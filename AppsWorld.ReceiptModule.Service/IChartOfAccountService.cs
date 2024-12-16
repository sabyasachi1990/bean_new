using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IChartOfAccountService : IService<ChartOfAccount>
    {
		ChartOfAccount GetChartOfAccountById(long id);

		List<ChartOfAccount> GetChartOfAccounts(long AccountTypeId);

		List<ChartOfAccount> GetCOAEdit(long id, long companyId);

        ChartOfAccount GetByName(string name, long companyId);
        List<ChartOfAccount> GetCashAndBankCOAId(long companyId, long accountid, long coaId);
        List<ChartOfAccount> GetCashAndBankActiveInactive(long companyId, long accountid, long coaId);

        List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId);
        long GetByNameId(string name, long companyId);
        long GetICAccountId(long companyId, long serviceCompanyId);
    }
}
