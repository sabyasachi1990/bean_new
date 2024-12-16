using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class AccountTypeService : Service<AccountType>, IAccountTypeService
    {
        private readonly IMasterModuleRepositoryAsync<AccountType> _accountTypeRepository;
        public AccountTypeService(IMasterModuleRepositoryAsync<AccountType> accountTypeService)
            : base(accountTypeService)
        {
            _accountTypeRepository = accountTypeService;
        }
        public IEnumerable<AccountType> GetAllAccountType(long AccTypeId, long CompanyId)
        {
            return _accountTypeRepository.Query(a => (a.Status == RecordStatusEnum.Active || a.Id == AccTypeId) && a.CompanyId == CompanyId).Select().ToList();
        }
        public IEnumerable<AccountType> GetAllAccountTypes(long companyId)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean" && (/*c.Name!= "Intercompany clearing"&& c.Name != "Intercompany billing" &&*/ c.Name != "System")).Select().AsEnumerable();
        }
        public IEnumerable<AccountType> GetAllAccountTypeByCidIssys(long companyId, bool IsSystem)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.IsSystem == IsSystem && c.Status == RecordStatusEnum.Active && c.ModuleType == "Bean").Select().AsEnumerable();
        }
        public IEnumerable<AccountType> GetAllAccountTypes()
        {
            return _accountTypeRepository.Query(a => a.Status < RecordStatusEnum.Disable).Select().AsEnumerable();
        }
        public long GetAllAccounyTypeByName(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name && c.Status == RecordStatusEnum.Active).Select(c => c.Id).FirstOrDefault();
        }
        public AccountType GetAllAccounyType(long companyId)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<long> GetAllAccounyTypeByNameByCOA(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Select(d => d.Id).ToList();
        }
        public List<long> GetAllNameByAccountType(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && !name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Select(d => d.Id).ToList();
        }
        public async Task<List<long>> GetAllNameByAccountTypeAsync(long companyId, List<string> name)
        {
            return await Task.Run(()=> _accountTypeRepository.Query(c => c.CompanyId == companyId && !name.Contains(c.Name) ).Select(d => d.Id).ToList());
        }
        public Guid? GetAllAccountTypeById(string name, long companyId)
        {
            return _accountTypeRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select(s => s.FRATId).FirstOrDefault();
        }
        public string GetAccountTypeName(long accountTypeId)
        {
            return _accountTypeRepository.Query(c => c.Id == accountTypeId).Select(c => c.Name).FirstOrDefault();
        }
        //public List<AccountType> GetAllAccounyTypeByNames(long companyId, List<string> name)
        //{
        //    return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Include(c => c.ChartOfAccounts).Select().ToList();
        //}

        public Dictionary<long, string> GetAllAccounyTypeIdNames(long companyId, List<string> name)
        {
            //commented on 08/04/2020
            //return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name)).Select(x => new { Id = x.Id, Name = x.Name }).ToDictionary(Id => Id.Id, Name => Name.Name);

            return _accountTypeRepository.Query(c => c.CompanyId == companyId && !name.Contains(c.Name)).Select(x => new { Id = x.Id, Name = x.Name }).ToDictionary(Id => Id.Id, Name => Name.Name);
        }
        //public List<AccountType> GetAllAccounyTypeByName(long companyId, List<string> name)
        //{
        //    return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Include(c => c.ChartOfAccounts).Select().ToList();
        //}
    }
}
