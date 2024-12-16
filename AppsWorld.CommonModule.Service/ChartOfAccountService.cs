using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Infra.Resources;

namespace AppsWorld.CommonModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly ICommonModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        private readonly ICommonModuleRepositoryAsync<Company> _companyRepository;
        private readonly ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUser> _companyUserRepository;
        private readonly ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;

        public ChartOfAccountService(ICommonModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, ICommonModuleRepositoryAsync<Company> companyRepository, ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUser> companyUserRepository, ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
            : base(chartOfAccountRepository)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
        }

        public ChartOfAccount GetChartOfAccountById(long id)
        {
            return _chartOfAccountRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public List<ChartOfAccount> GetAllChartOfAccountsByUsername(long CompanyId, string username)
        {
            return (from b in _chartOfAccountRepository.Queryable()
                    join c in _companyRepository.Queryable() on b.CompanyId equals c.ParentId
                    where c.ParentId == CompanyId
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == CompanyId && b.IsRealCOA == true && b.Status < RecordStatusEnum.Disable && (b.SubsidaryCompanyId == c.Id || b.SubsidaryCompanyId == null) && cu.Username == username
                    select b).Distinct().ToList();
        }
        public List<ChartOfAccount> GetChartOfAccounts(long accountTypeId, long coaId)
        {
            return _chartOfAccountRepository.Query(c => c.AccountTypeId == accountTypeId && (c.Status == RecordStatusEnum.Active || c.Id == coaId) && c.ModuleType == "Bean").Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public List<ChartOfAccount> GetChartOfActiveInactiveBanksAccounts(long companyId, long accountTypeId)
        {
            return _chartOfAccountRepository.Query(c => c.AccountTypeId == accountTypeId && (c.Status == RecordStatusEnum.Active || c.Status == RecordStatusEnum.Inactive) && c.ModuleType == "Bean" && c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public List<ChartOfAccount> GetCOAEdit(long id, long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => (c.Status == RecordStatusEnum.Active || c.Id == id) && c.CompanyId == companyId && c.ModuleType == "Bean" && c.IsSystem == false).OrderByDescending(c => c.CreatedDate).ToList();
        }
        //public ChartOfAccount GetChartOfAccountByCId(long companyId)
        //{
        //    return _chartOfAccountRepository.Query(a => a.Name == COANameConstants.Clearing_Receipts && a.CompanyId == companyId && a.ModuleType == "Bean" && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        //}
        public List<ChartOfAccount> GetAllChartOfAccounts(long CompanyId)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == CompanyId && c.Status == RecordStatusEnum.Active && c.ModuleType == "Bean").Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public ChartOfAccount GetChartOfAccountByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public ChartOfAccount GetChartOfAccount(long id)
        {
            return _chartOfAccountRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }
        public ChartOfAccount GetChartOfAccountByCompanyId(long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == COANameConstants.TaxPayableGST && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public long GetChartOfAccountByNature(string nature, long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == (nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables) && a.CompanyId == CompanyId).Select(d => d.Id).FirstOrDefault();
        }
        public long? GetChartOfAccountIDByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select(c => c.Id).FirstOrDefault();
        }
        public List<ChartOfAccount> GetCashAndBankCOAId(long companyId, long accountid)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.AccountTypeId == accountid && a.Status == RecordStatusEnum.Active).Select().ToList();

        }
        public List<ChartOfAccount> lstchartofaccount(long accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => a.AccountTypeId == accountTypeId).Select().ToList();
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
                    IsDefault = x.Name == COANameConstants.ExchangeGainLossRealised ? true : false,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false
                }).OrderBy(x => x.Name).ToList();
                return listchart;
            }
        }
        public List<COALookup<string>> ListCOADetail(long companyId, bool isEdit)
        {
            if (isEdit)
            {
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive) && a.CompanyId == companyId && a.ModuleType == "Bean" && (a.IsSystem == false || a.IsSystem == null) && a.IsRealCOA == true).Select(x => new COALookup<string>()
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
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == companyId && a.ModuleType == "Bean" && (a.IsSystem == false || a.IsSystem == null) && a.IsRealCOA == true).Select(x => new COALookup<string>()
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
        public List<ChartOfAccount> GetAllBalanceSheet(long companyId, string name)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == companyId && c.Category == name && c.Status == RecordStatusEnum.Active && c.ModuleType == "Bean" /*&& (c.IsSystem == false || c.IsSystem == null)*/ && c.IsRealCOA == true).Select().ToList();
        }
        public List<ChartOfAccount> GellAllCashByCurrency(long companyId, long? serviceCompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.SubsidaryCompanyId == serviceCompanyId).Select().ToList();
        }
        public List<ChartOfAccount> GetAllCOAByBank(long companyId, List<long> lstCOAId, List<long> serviceCompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.IsSeedData == false && !lstCOAId.Contains(a.Id) && (a.SubsidaryCompanyId == null || serviceCompanyId.Contains((Int64)(a.SubsidaryCompanyId)))  /*&& (a.SubsidaryCompanyId != serviceCompanyId || a.SubsidaryCompanyId == null)*/).Select().ToList();
        }
        public List<ChartOfAccount> GetAllCOAById(long companyId, List<long> lstCoaId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => lstCoaId.Contains(c.Id) && c.CompanyId == companyId).ToList();
        }
        public string GetCOAName(long coaId, long companyId)
        {
            ChartOfAccount coa = _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.Id == coaId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
            return coa != null ? coa.Name : null;
        }
        public List<COALookup<string>> GLClearingCOAs(long companyId, bool iSedit)
        {
            if (iSedit)
            {
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive) && a.CompanyId == companyId && a.ModuleType == "Bean" && a.IsRealCOA == true && (a.Name != COANameConstants.AccountsPayable && a.Name != COANameConstants.OtherPayables && a.Name != COANameConstants.AccountsReceivables && a.Name != COANameConstants.OtherReceivables)).Select(x => new COALookup<string>()
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
                List<COALookup<string>> listchart = _chartOfAccountRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == companyId && a.ModuleType == "Bean" && a.IsRealCOA == true && (a.Name != COANameConstants.AccountsPayable && a.Name != COANameConstants.OtherPayables && a.Name != COANameConstants.AccountsReceivables && a.Name != COANameConstants.OtherReceivables)).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsDefault = x.Name == COANameConstants.ExchangeGainLossRealised ? true : false,
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
        public List<ChartOfAccount> GetAllCOAByIds(List<long> lstCoaId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => lstCoaId.Contains(c.Id)).ToList();
        }

        public List<long> GetAllCOAByIdsAndALE(List<long> lstCoaId, string classes)
        {
            return _chartOfAccountRepository.Queryable().Where(z => lstCoaId.Contains(z.Id) && classes.Contains(z.Class)).Select(z => z.Id).ToList();
        }

        public ChartOfAccount GetWorkFolwItemByCoaId(long? companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(z => z.CompanyId == companyId && z.Name == InvoiceConstants.Rounding_Gst).FirstOrDefault();
        }
        public List<long> GetAllCOAIdsByIds(List<long> lstCoaId)
        {
            return _chartOfAccountRepository.Queryable().Where(c => lstCoaId.Contains(c.Id)).Select(c => c.Id).ToList();
        }
        public List<ChartOfAccount> GetAllCOAOfBank(long companyId, List<long> lstCOAId, long serviceCompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.IsSeedData == false && a.SubsidaryCompanyId == serviceCompanyId && a.IsBank == true).Select().ToList();
        }
        public List<long> GetCOAIDsByName(List<String> coaNames, long companyId)
        {
            return _chartOfAccountRepository.Queryable().Where(a => a.CompanyId == companyId && coaNames.Contains(a.Name)).Select(a => a.Id).ToList();
        }
        public long? GetCoaIdByNameAndCompanyId(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select(a => a.Id).FirstOrDefault();
        }
        public Dictionary<long, string> GetChartofAccounts(List<string> Name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => Name.Contains(a.Name) && a.CompanyId == companyId).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        }
        public Dictionary<long, string> GetAllChartofAccountsById(List<long> Ids, long companyId)
        {
            return _chartOfAccountRepository.Query(a => Ids.Contains(a.Id) && a.CompanyId == companyId).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        }
        public long GetICAccountId(long companyId, long serviceCompanyId)
        {
            return _chartOfAccountRepository.Query(c => c.SubsidaryCompanyId == serviceCompanyId && c.Name.Contains("I/C")).Select(c => c.Id).FirstOrDefault();
        }
        public ChartOfAccount GetByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Name == name && c.CompanyId == companyId).Select().FirstOrDefault();
        }
    }
}
