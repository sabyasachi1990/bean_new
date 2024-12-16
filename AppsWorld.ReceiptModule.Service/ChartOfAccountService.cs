using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.ReceiptModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly IReceiptModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;

        public ChartOfAccountService(IReceiptModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository)
            : base(chartOfAccountRepository)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
        }

        public ChartOfAccount GetChartOfAccountById(long id)
        {
            return _chartOfAccountRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }

        public List<ChartOfAccount> GetChartOfAccounts(long accountTypeId)
        {
            return _chartOfAccountRepository.Query(c => c.AccountTypeId == accountTypeId && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean" && c.IsRealCOA == true).Select().OrderByDescending(c => c.CreatedDate).OrderBy(c => c.Name).ToList();
        }
        public List<ChartOfAccount> GetCOAEdit(long id, long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => (c.Status == RecordStatusEnum.Active || c.Id == id) && c.CompanyId == companyId && c.ModuleType == "Bean" && c.IsSystem == false && c.IsRealCOA == true).OrderByDescending(c => c.CreatedDate).OrderBy(c => c.Name).ToList();
        }
        public ChartOfAccount GetByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<ChartOfAccount> GetCashAndBankCOAId(long companyId, long accountid, long coaId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.AccountTypeId == accountid && (a.Status == RecordStatusEnum.Active || a.Id == coaId)).Select().ToList();

        }
        public List<ChartOfAccount> GetCashAndBankActiveInactive(long companyId, long accountid, long coaId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.AccountTypeId == accountid && ((a.Status == RecordStatusEnum.Active && a.Status == RecordStatusEnum.Inactive) || a.Id == coaId)).Select().ToList();

        }
        public List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => accountTypeId.Contains(a.AccountTypeId)).Select().ToList();
        }
        public long GetByNameId(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select(x=>x.Id).FirstOrDefault();
        }
        public long GetICAccountId(long companyId,long serviceCompanyId)
        {
            return _chartOfAccountRepository.Query(c => c.SubsidaryCompanyId == serviceCompanyId && c.Name.Contains("I/C")).Select(c => c.Id).FirstOrDefault();
        }
    }
}
