using Service.Pattern;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using System;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;

        public ChartOfAccountService(IJournalVoucherModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository)
            : base(chartOfAccountRepository)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
        }

        public ChartOfAccount GetChartOfAccountById(long id)
        {
            return _chartOfAccountRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public ChartOfAccount GetCOAEdit(long id, long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => c.Id == id && c.CompanyId == companyId && c.ModuleType == "Bean" /*&& c.IsSystem == false*/&& c.IsRealCOA == true).FirstOrDefault();
        }
        public List<ChartOfAccount> GetCOANew(long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Status == RecordStatusEnum.Active && c.CompanyId == companyId && c.ModuleType == "Bean"/* && (c.IsSystem == false || c.IsSystem == null)*/&& c.IsRealCOA == true).Select().ToList();
        }
        public ChartOfAccount GetByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => accountTypeId.Contains(a.AccountTypeId)).Select().ToList();
        }
        public List<ChartOfAccount> GetChartofAccountsByCompanyId(long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => c.CompanyId == companyId).ToList();
        }
        public List<ChartOfAccount> GetCOAbycategoryid(Guid categoryId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => c.CategoryId == categoryId).ToList();
        }
        public ChartOfAccount GetById(Guid id)
        {
            return _chartOfAccountRepository.Query(c => c.FRCoaId == id).Select().FirstOrDefault();
        }
        public ChartOfAccount GetByIdLid(Guid id, Guid value)
        {
            return _chartOfAccountRepository.Query(c => c.FRCoaId == id && c.FRPATId == value).Select().FirstOrDefault();
        }

        public Dictionary<long, string> GetChartofAccounts(List<long> coaIds, long companyId)
        {
            return _chartOfAccountRepository.Query(a => coaIds.Contains(a.Id) && a.CompanyId == companyId).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        }
        public long GetByNameAndCompanyId(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select(a => a.Id).FirstOrDefault();
        }

        public Dictionary<long, long?> GetListChartofAccounts(List<long> accTypeIds, long companyId, List<long> servEntityIds)
        {
            return _chartOfAccountRepository.Query(a => accTypeIds.Contains(a.AccountTypeId) && a.CompanyId == companyId && a.IsRevaluation != 1 && servEntityIds.Contains(a.SubsidaryCompanyId.Value)).Select(a => new { a.Id, a.SubsidaryCompanyId }).ToDictionary(t => t.Id, t => t.SubsidaryCompanyId);
        }
    }
}
