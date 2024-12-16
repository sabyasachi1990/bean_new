using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly IBillModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;

        public ChartOfAccountService(IBillModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository)
            : base(chartOfAccountRepository)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
        }

        public ChartOfAccount GetChartOfAccountById(long id)
        {
            return _chartOfAccountRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public List<ChartOfAccount> GetCOAEdit(long id, long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => (c.Status == RecordStatusEnum.Active || c.Id == id) && c.CompanyId == companyId && c.ModuleType == "Bean").ToList();
        }
        public List<ChartOfAccount> GetCOANew(long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => c.Status == RecordStatusEnum.Active && c.CompanyId == companyId && c.ModuleType == "Bean").ToList();
        }

        public ChartOfAccount GetByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<COALookup<string>> listOfChartOfAccounts(long companyId, bool iSedit)
        {
            if (iSedit)
            {
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive) && a.CompanyId == companyId && a.ModuleType == "Bean" && a.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false
                }).OrderBy(x => x.Name).ToList();
                return listchart;
            }
            else
            {
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == companyId && a.ModuleType == "Bean" && a.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false
                }).OrderBy(x => x.Name).ToList();
                return listchart;
            }
        }
        public List<ChartOfAccount> GetChartOfAccountByAccountType(List<long> accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => accountTypeId.Contains(a.AccountTypeId)).Select().ToList();
        }
    }
}
