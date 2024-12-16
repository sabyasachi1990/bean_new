using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;

using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public class AccountTypeService : Service<AccountType>, IAccountTypeService
    {
        private readonly IBillModuleRepositoryAsync<AccountType> _accountTypeRepository;
        public AccountTypeService(IBillModuleRepositoryAsync<AccountType> accountTypeService)
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
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean").Select().AsEnumerable();
        }
        public IEnumerable<AccountType> GetAllAccountTypeByCidIssys(long companyId, bool IsSystem)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.IsSystem == IsSystem && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean").Select().AsEnumerable();
        }
        public IEnumerable<AccountType> GetAllAccountTypes()
        {
            return _accountTypeRepository.Query(a => a.Status < RecordStatusEnum.Disable).Select().AsEnumerable();
        }
        public AccountType GetAllAccounyTypeByName(long companyId, string name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name==name && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<long> GetAllAccounyTypeByName(long companyId, List<string> name)
        {
            return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) && c.Status == RecordStatusEnum.Active).Select(x=>x.Id).ToList();
        }
    }
}
