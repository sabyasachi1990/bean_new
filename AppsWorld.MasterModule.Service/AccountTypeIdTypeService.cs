using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class AccountTypeIdTypeService : Service<AccountTypeIdType>, IAccountTypeIdtypeService
    {
        private readonly IMasterModuleRepositoryAsync<AccountTypeIdType> _accountTypeIdtypRepository;
        public AccountTypeIdTypeService(IMasterModuleRepositoryAsync<AccountTypeIdType> accountTypeIdtypRepository)
            : base(accountTypeIdtypRepository)
        {
            _accountTypeIdtypRepository = accountTypeIdtypRepository;
        }
        public List<AccountTypeIdType> GetAccountTypeIdTypes(long accountTypeId)
        {
            return _accountTypeIdtypRepository.Query(c => c.AccountTypeId == accountTypeId && c.IdType.Status <= RecordStatusEnum.Inactive).Include(c => c.IdType).Select().ToList();
        }

    }
}
