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
using AppsWorld.CommonModule.Entities.Models;

namespace AppsWorld.CommonModule.Service
{
    public class CompanyService : Service<Company>, ICompanyService
    {
        private readonly ICommonModuleRepositoryAsync<Company> _companyRepository;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly ICommonModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly ICommonModuleRepositoryAsync<CompanyUserDetail> _companyUserDetailRepository;
        // private readonly ICompanyService _companyService;

        public CompanyService(ICommonModuleRepositoryAsync<Company> companyRepository, IAccountTypeService accountTypeService, IChartOfAccountService chartOfAccountService, IFinancialSettingService financialSettingService, ICommonModuleRepositoryAsync<CompanyUser> companyUserRepository, ICommonModuleRepositoryAsync<CompanyUserDetail> companyUserDetailRepository)
            : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _accountTypeService = accountTypeService;
            _chartOfAccountService = chartOfAccountService;
            this._financialSettingService = financialSettingService;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;

        }
        //public Company GetByNameByServiceCompany(long parentId)
        //{
        //    return _companyRepository.Query(c => c.ParentId == parentId).Select().FirstOrDefault();
        //}
        public string GetByNameByServiceCompany(long parentId)
        {
            return _companyRepository.Query(c => c.ParentId == parentId).Select(a => a.ShortName).FirstOrDefault();
        }
        public List<Company> GetCompany(long companyId, long? companyIdCheck, string username)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyIdCheck) && c.ParentId == companyId && cu.Username == username
                    select c).ToList();
        }
  
        public Company GetById(long id)
        {
            return _companyRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public string GetByName(long id)
        {
            return _companyRepository.Query(c => c.Id == id).Select(s => s.ShortName).FirstOrDefault();
        }
        public List<Company> GetServiceCompany(long parentId, long companyId)
        {
            return _companyRepository.Queryable().AsEnumerable().Where(a => (a.Status == RecordStatusEnum.Active || a.ParentId == parentId) && a.Id == companyId).ToList();
        }
        public Company GetCompanyByCompanyid(long companyId)
        {
            return _companyRepository.Query(a => a.Id == companyId).Select().FirstOrDefault();
        }
        public string GetIdBy(long Id)
        {
            return _companyRepository.Query(a => a.Id == Id).Select(a => a.ShortName).FirstOrDefault();
        }
        public List<Company> GetlstCompany(long id)
        {
            return _companyRepository.Query(c => c.ParentId == id).Select().ToList();
        }
        public List<Company> GetAllCompanies(List<long> ids)
        {
            return _companyRepository.Query(c => ids.Contains(c.Id)).Select().ToList();
        }
        public List<LookUpCompany<string>> Listofsubsudarycompany(string username, long companyId, long? subcompanyid)
        {
            //long comp = cashSale == null ? 0 : cashSale== null ? 0 : cashSale.CompanyId;
            FinancialSetting financial = _financialSettingService.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
            AccountType account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
            //var listcoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
            //var companyData = _companyRepository.Query(c => c.Id == companyId).Select().FirstOrDefault();
            return GetCompany(companyId, subcompanyid, username).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                //IsBaseCompany = x.Name == companyData.Name ? true : false,
                isGstActivated = x.IsGstSetting,
                LookUps = account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (/*c.IsSystem == false || */c.IsLinkedAccount != null)).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    // Name = x.ShortName + '-' + a.Name,                    
                    Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = x.Id,
                }).ToList()
            }).ToList();
            //return lstCompany;
        }
        public List<LookUp<string>> CashAndBankCurrency(long companyId, long? subcompanyId, string currency)
        {
            var listcoa = _chartOfAccountService.GellAllCashByCurrency(companyId, subcompanyId);
            var lstLookUp = listcoa.Where(c => c.SubsidaryCompanyId == subcompanyId && c.Currency == currency).Select(x => new CommonModule.Infra.LookUp<string>
            {
                Id = x.Id,
                Name = x.Name,
                Currency = x.Currency,
                Code = x.Code,
                ServiceCompanyId = x.SubsidaryCompanyId.Value
            }).ToList();
            return lstLookUp;
        }
        public List<LookUpCompany<string>> GetSubCompany(string username, long companyId, long? subcompanyid)
        {
            return GetCompany(companyId, subcompanyid, username).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                isGstActivated = x.IsGstSetting
            }).OrderBy(x => x.ShortName).ToList();
            //return lstCompany;
        }

        public List<LookUpCompany<string>> Listofsubsidarycompany(long companyId, long? subcompanyid, List<long> COAIds, Guid bankRecId, string userName)
        {
            //FinancialSetting financial = _financialSettingService.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
            string baseCurrency = _financialSettingService.Query(c => c.CompanyId == companyId).Select(a => a.BaseCurrency).FirstOrDefault();
            AccountType account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
            //var listcoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
            //var companyData = _companyRepository.Query(c => c.Id == companyId).Select().FirstOrDefault();
            return GetCompany(companyId, subcompanyid, userName).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                //IsBaseCompany = x.Name == companyData.Name ? true : false,
                //LookUps = account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (c.IsSystem == false || c.IsSystem == null) && c.IsBank == true).Select(a => new CommonModule.Infra.LookUp<string>()
                //{
                //    Id = a.Id,
                //    // Name = x.ShortName + '-' + a.Name,                    
                //    Name = a.Name,
                //    //Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                //    Currency = a.Currency != null ? a.Currency : baseCurrency,
                //    Code = a.Code,
                //    ServiceCompanyId = a.SubsidaryCompanyId,
                //}).ToList()

                LookUps = bankRecId == new Guid() ? account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && c.Status == RecordStatusEnum.Active && (c.IsSystem == false || c.IsSystem == null) && c.IsBank == true).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Currency = a.Currency != null ? a.Currency : baseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = a.SubsidaryCompanyId,
                }).OrderBy(d => d.Name).ToList() : account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (COAIds.Contains(c.Id) || c.Status == RecordStatusEnum.Active) && (c.IsSystem == false || c.IsSystem == null) && c.IsBank == true).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Currency = a.Currency != null ? a.Currency : baseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = a.SubsidaryCompanyId,
                }).OrderBy(d => d.Name).ToList()


            }).OrderBy(x => x.ShortName).ToList();
            //return lstCompany;
        }
        public List<LookUpCompany<string>> ListOfSubsudaryCompanyActiveInactive(long companyId, long? subcompanyid, Guid Id, List<long> COAIds, string userName)
        {
            //long comp = cashSale == null ? 0 : cashSale== null ? 0 : cashSale.CompanyId;
            //List<ChartOfAccount> listcoa = new List<ChartOfAccount>();
            FinancialSetting financial = _financialSettingService.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
            AccountType account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
            //if (Id == new Guid())
            //    listcoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
            //else
            //    listcoa = _chartOfAccountService.GetChartOfActiveInactiveBanksAccounts(companyId, account.Id);
            //var companyData = _companyRepository.Query(c => c.Id == companyId).Select().FirstOrDefault();
            var lstCompany = GetCompany(companyId, subcompanyid, userName).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                isGstActivated = x.IsGstSetting,
                //IsBaseCompany = x.Name == companyData.Name ? true : false,
                LookUps = Id == new Guid() ? account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && c.IsLinkedAccount != null && c.IsRevaluation == null && c.Status == RecordStatusEnum.Active).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    // Name = x.ShortName + '-' + a.Name,                    
                    Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = x.Id,
                    Status = a.Status
                }).OrderBy(d => d.Name).ToList() : account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (COAIds.Contains(c.Id) || c.Status == RecordStatusEnum.Active) && (/*c.IsSystem == false || */c.IsLinkedAccount != null && c.IsRevaluation == null)).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    // Name = x.ShortName + '-' + a.Name,                    
                    Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = x.Id,
                    Status = a.Status
                }).OrderBy(d => d.Name).ToList()
            }).ToList();
            return lstCompany.OrderBy(x => x.ShortName).ToList();
        }

        public async Task<List<LookUpCompany<string>>> ListOfSubsudaryCompanyActiveInactiveAsync(long companyId, long? subcompanyid, Guid Id, List<long> COAIds, string userName)
        {
            FinancialSetting financial =  await Task.Run(()=> _financialSettingService.Query(c => c.CompanyId == companyId).Select().FirstOrDefault());
            AccountType account = await _accountTypeService.GetByIdAsync(companyId, AccountNameConstants.Cash_and_bank_balances);
            var lstCompany = GetCompany(companyId, subcompanyid, userName).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                isGstActivated = x.IsGstSetting,
                LookUps = Id == new Guid() ? account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && c.IsLinkedAccount != null && c.IsRevaluation == null && c.Status == RecordStatusEnum.Active).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,                 
                    Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = x.Id,
                    Status = a.Status
                }).OrderBy(d => d.Name).ToList() : account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (COAIds.Contains(c.Id) || c.Status == RecordStatusEnum.Active) && (/*c.IsSystem == false || */c.IsLinkedAccount != null && c.IsRevaluation == null)).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,              
                    Name = ((a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) || ((a.Currency != null && a.Currency != string.Empty) && a.IsBank.Equals(null))) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = x.Id,
                    Status = a.Status
                }).OrderBy(d => d.Name).ToList()
            }).ToList();
            return lstCompany.OrderBy(x => x.ShortName).ToList();
        }

        public CompanyUser GetCompanyUserByCid_User(string username, long companyId)
        {
            return _companyUserRepository.Queryable().FirstOrDefault(z => z.CompanyId == companyId && z.Username == username);
        }

        public List<string> GetServiceCompanyNameById(List<long> serviceCompIds)
        {
            return _companyRepository.Queryable().Where(z => serviceCompIds.Contains(z.Id)).Select(z => z.ShortName).ToList();
        }


        public Dictionary<long, string> GetAllCompaniesName(List<long> Ids)
        {
            return _companyRepository.Query(c => Ids.Contains(c.Id)).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
        }
        public Dictionary<long, string> GetAllSubCompanies(List<long> Ids, string username, long companyId)
        {
            return (from c in _companyRepository.Query(c => Ids.Contains(c.Id)).Select().AsQueryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c).ToList().Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, nameof => nameof.Code);
        }
        public Dictionary<long, string> GetAllSubCompany(string username, long companyId)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c).ToList().Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, nameof => nameof.Code);
        }
        public List<long> GetAllSubCompaniesId(string username, long companyId)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c.Id).ToList();
        }
        public bool? GetServiceCompanyStatusByUsername(long servId, long companyId, string username)
        {
            return (from c in _companyRepository.Queryable().Where(x=>x.Id == servId)
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where cu.CompanyId == companyId && cu.Username == username
                    select c.Id).Any();
        }
        public bool GetPermissionBasedOnUser(long? serviceEntityId, long? companyId,string username)
        {
            return (from cu in _companyUserRepository.Queryable()
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where cud.ServiceEntityId == serviceEntityId && cu.CompanyId == companyId && cu.Username == username
                    select cud.ServiceEntityId).Any();
        }
        public Dictionary<long, RecordStatusEnum> GetAllCompaniesStatus(List<long> Ids)
        {
            return _companyRepository.Query(c => Ids.Contains(c.Id)).Select(c => new { Ids = c.Id, Status = c.Status }).ToDictionary(Id => Id.Ids, Status => Status.Status);
        }
        public Dictionary<long, string> GetAllCompaniesNameByParentId(long parentId, bool? isInterCompanyActivate, bool isEdit)
        {
            if (isEdit)
            {
                if (isInterCompanyActivate == true)
                    return _companyRepository.Query(c => c.ParentId == parentId).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
                else
                    return _companyRepository.Query(c => c.ParentId == parentId).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
            }
            else
            {
                if (isInterCompanyActivate == true)
                    return _companyRepository.Query(c => c.ParentId == parentId && c.Status == RecordStatusEnum.Active).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
                else
                {
                    return _companyRepository.Query(c => c.ParentId == parentId && c.Status == RecordStatusEnum.Active).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
                }
            }
        }
    }
}
