
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly IMasterModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        private readonly IMasterModuleRepositoryAsync<AccountType> _accountTypeRepository;
        private readonly IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company> _companyRepository;
        private readonly IMasterModuleRepositoryAsync<CompanyUser> _compUserRepository;
        private readonly IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepository;
        public ChartOfAccountService(IMasterModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, IMasterModuleRepositoryAsync<AccountType> accountTypeRepository, IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company> companyRepository, IMasterModuleRepositoryAsync<CompanyUser> compUserRepository, IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepository)
            : base(chartOfAccountRepository)
        {
            this._chartOfAccountRepository = chartOfAccountRepository;
            _accountTypeRepository = accountTypeRepository;
            _companyRepository = companyRepository;
            _compUserRepository = compUserRepository;
            _compUserDetailRepository = compUserDetailRepository;
        }
        public List<ChartOfAccount> GetChartOfAccountByDisable(long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.Status == RecordStatusEnum.Disable && a.CompanyId == CompanyId/* && !a.Name.Contains("Inventory revaluation")*/).Include(a => a.AccountType).Select().ToList();
        }
        public List<ChartOfAccount> GetChartOfAccountByRevaluation(long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.IsRevaluation == 1 && a.CompanyId == CompanyId /*&& !a.Name.Contains("Inventory revaluation")*/).Select().ToList();
        }
        public IQueryable<ChartOfAccount> GetChartOfAccountBycid(long CompanyId)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == CompanyId && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean" && c.IsRealCOA == true).Include(ac => ac.AccountType).Select().AsQueryable();
        }
        public List<ChartOfAccount> GetChartOfAccountByIdcid(long chartofaccountid, long CompanyId)
        {
            return _chartOfAccountRepository.Query(e => e.Id == chartofaccountid && e.CompanyId == CompanyId).Select().ToList();
        }
        public List<ChartOfAccount> GetChartOfAccountBycidcode(string code, long CompanyId)
        {
            return _chartOfAccountRepository.Query(e => e.Code.ToLower() == code.ToLower() && e.CompanyId == CompanyId).Select().ToList();
        }
        public List<ChartOfAccount> GetChartOfAccountBycidIdCode(long coaid, string code, long CompanyId)
        {
            return _chartOfAccountRepository.Query(e => e.Id == coaid && e.Code.ToLower() == code.ToLower() && e.CompanyId == CompanyId).Select().ToList();
        }
        public List<COALookup<string>> GetChartOfAccountBycidAndId(long coa, long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Id == coa) && a.CompanyId == CompanyId && a.ModuleType == "Bean" && a.IsRealCOA == true).Select(x => new COALookup<string>()
            {
                Name = x.Name,
                Id = x.Id,
                RecOrder = x.RecOrder,
                IsAllowDisAllow = x.DisAllowable ?? false,
                IsPLAccount = x.Category == "Income Statement" ? true : false
            }).AsEnumerable().OrderBy(x => x.Name).ToList();
        }
        public List<COALookup<string>> GetChartOfAccountByCId(long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId && a.ModuleType == "Bean" && a.IsRealCOA == true).Select(x => new COALookup<string>()
            {
                Name = x.Name,
                Id = x.Id,
                RecOrder = x.RecOrder,
                IsAllowDisAllow = x.DisAllowable ?? false,
                IsPLAccount = x.Category == "Income Statement" ? true : false
            }).AsEnumerable().OrderBy(x => x.Name).ToList();

        }
        public ChartOfAccount GetChartOfAccountById(long id)
        {
            return _chartOfAccountRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }
        public ChartOfAccount CheckName(long id, string name, long companyId)
        {
            return _chartOfAccountRepository.Query(c => c.Id != id && c.CompanyId == companyId && c.Name.ToLower() == name.ToLower() && c.ModuleType == "Bean").Select().FirstOrDefault();
        }

        public List<ChartOfAccount> CheckCode(string code, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.Code.ToLower() == code.ToLower()).Select().ToList();
        }
        public List<long> GetChartOfAccountId(long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.IsRevaluation == 1 && a.Status == RecordStatusEnum.Active).Select(c => c.Id).ToList();
        }
        public string GetCOAName(long coaId, long companyId)
        {
            ChartOfAccount coa = _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.Id == coaId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
            return coa != null ? coa.Name : null;
        }
        public List<ChartOfAccount> GetChartOfAccountByAccountTypeCOA(List<long> accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => accountTypeId.Contains(a.AccountTypeId)).Select().ToList();
        }

        public async Task<List<ChartOfAccount>> GetChartOfAccountByAccountTypeCOAAsync(List<long> accountTypeId)
        {
            return await Task.Run(()=> _chartOfAccountRepository.Query(a => accountTypeId.Contains(a.AccountTypeId)).Select().ToList());
        }
        public List<ChartOfAccount> GetChartOfAccountByAccountType(long accountTypeId)
        {
            return _chartOfAccountRepository.Query(a => a.AccountTypeId == accountTypeId).Select().ToList();
        }
        public ChartOfAccount GetOBChartOfdAccount(long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && a.Name == "Opening balance").Select().FirstOrDefault();
        }
        public async Task<IQueryable<ChartOfAccountModelK>> GetAllCOAK(long companyId, string username)
        {
           
            IQueryable<AccountType> accTypes = await Task.Run(()=>  _accountTypeRepository.Queryable().Where(c => c.CompanyId == companyId));
            return (from b in await Task.Run(()=> _chartOfAccountRepository.Queryable())
                        join c in await Task.Run(()=> _companyRepository.Queryable()) on b.CompanyId equals c.ParentId where c.ParentId == companyId
                        join cu in await Task.Run(()=> _compUserRepository.Queryable()) on c.ParentId equals cu.CompanyId
                        join cud in await Task.Run(()=> _compUserDetailRepository.Queryable()) on cu.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && b.IsRealCOA == true && b.Status < RecordStatusEnum.Disable && (b.SubsidaryCompanyId == c.Id || b.SubsidaryCompanyId == null)
                    && cu.Username == username
                    select new ChartOfAccountModelK()
                    {
                        Id = b.Id,
                        CompanyId = b.CompanyId,
                        Name = b.Name,
                        Code = b.Code,
                        IsSystem = b.IsSystem,
                        AccountTypeName = accTypes.Where(c => c.Id == b.AccountTypeId).Select(d => d.Name).FirstOrDefault(),
                        Status = b.Status.ToString(),
                        Category = b.Category,
                        Class = b.Class,
                        SubCategory = b.SubCategory,
                        Nature = b.Nature,
                        CreatedDate = b.CreatedDate,
                        UserCreated = b.UserCreated,
                        ModifiedBy = b.ModifiedBy,
                        ModifiedDate = b.ModifiedDate,
                        DisAllowable = b.DisAllowable,
                        Revaluation = b.Revaluation,
                        ShowRevaluation = b.ShowRevaluation,
                        ModuleType = b.ModuleType,
                        CashflowType = b.CashflowType,
                        IsLinkedAccount = b.IsLinkedAccount,
                    }).Distinct().OrderBy(a => a.Code).AsQueryable();
        }
        public ChartOfAccount GetChartOfAccount(long companyId, long coaId)
        {
            return _chartOfAccountRepository.Queryable().Where(s => s.CompanyId == companyId && s.Id == coaId).FirstOrDefault();
        }
    }
}
