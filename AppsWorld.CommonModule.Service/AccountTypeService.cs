using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public class AccountTypeService : Service<AccountType>, IAccountTypeService
    {
        private readonly ICommonModuleRepositoryAsync<AccountType> _accountTypeRepository;
        public AccountTypeService(ICommonModuleRepositoryAsync<AccountType> accountTypeRepository)
            : base(accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }
        public AccountType GetById(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name && c.Status == RecordStatusEnum.Active).Include(c => c.ChartOfAccounts).Select().FirstOrDefault();
        }
        public async Task<AccountType> GetByIdAsync(long companyId, string name)
        {
            return await Task.Run(()=> _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name && c.Status == RecordStatusEnum.Active).Include(c => c.ChartOfAccounts).Select().FirstOrDefault());
        }
        public AccountType GetAccountTypeId(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Category == name && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<AccountType> GetAllAccounyTypeByName(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Include(c => c.ChartOfAccounts).Select().ToList();
        }
        public List<long> GetAllAccounyTypeByNameByID(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) && c.Status == RecordStatusEnum.Active).Select(x => x.Id).ToList();
        }
        public async Task<List<AccountType>> GetAllAccounyType(long companyId)
        {
            return await Task.Run(()=> _accountTypeRepository.Query(c => c.CompanyId == companyId && (c.Name != "System" && c.Name != "Intercompany clearing")).Include(c => c.ChartOfAccounts).Select().ToList());
        }
        public List<AccountType> GetAllAccountTypeNameByCompanyId(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && !name.Contains(c.Name)).Include(c => c.ChartOfAccounts).Select().ToList();
        }
        public  async Task<List<AccountType>> GetAllAccountTypeNameByCompanyIdAysnc(long companyId, List<string> name)
        {
            return await Task.Run(()=> _accountTypeRepository.Query(c => c.CompanyId == companyId && !name.Contains(c.Name)).Include(c => c.ChartOfAccounts).Select().ToList());
        }
        public List<AccountType> GetAllAccounyTypesForClearing(long companyId, List<string> classes)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && classes.Contains(c.Class) /*&& c.Status == RecordStatusEnum.Active*/&& c.Name != "Intercompany clearing").Include(d => d.ChartOfAccounts).Select().ToList();
        }
        public AccountType GetCashBankAccountbyName(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name && c.Status == RecordStatusEnum.Active).Include(c => c.ChartOfAccounts).Select().FirstOrDefault();
        }
        public AccountType GetAccountByName(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name).Include(c => c.ChartOfAccounts).Select().FirstOrDefault();
        }
        //public List<COALookup<string>> GetAllAccounyTypeByName(long companyId,string docType)
        //{
        //    List<string> accountTypeNames = null;
        //    if (docType == "Invoice")
        //        accountTypeNames = new List<string> { COANameConstants.Revenue };
        //    return _accountTypeRepository.Queryable().Where(c => c.CompanyId == companyId && accountTypeNames.Contains(c.Name) && c.Status == RecordStatusEnum.Active).(c => c.ChartOfAccounts).SelectMany(c => c.ChartOfAccounts.Select(x => new COALookup<string>()
        //    {
        //        Name = x.Name,
        //        Id = x.Id,
        //        RecOrder = x.RecOrder,
        //        IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //        IsPLAccount = x.Category == "Income Statement" ? true : false,
        //        Class = c.Class
        //    }).ToList()).ToList();
        //}
        public List<AccountType> GetAllAccountType(long companyId, List<long> lstAccId)
        {
            return _accountTypeRepository.Query(a => a.CompanyId == companyId && lstAccId.Contains(a.Id)).Select().ToList();
        }
        public List<AccountType> GetLeadSheetByCid(long companyid, bool v)
        {
            List<string> lststrings;
            if (v)
                lststrings = new List<string> { "Income", "Expenses" };
            else
                lststrings = new List<string> { "Assets", "Liabilities", "Equity" };

            return _accountTypeRepository.Query(c => c.CompanyId == companyid && lststrings.Contains(c.Class)).Select().OrderBy(a => a.RecOrder).ToList();

        }
    }
}
