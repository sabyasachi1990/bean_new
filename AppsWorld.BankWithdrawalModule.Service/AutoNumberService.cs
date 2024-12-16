using AppsWorld.BankWithdrawalModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.BankWithdrawalModule.Service
{
    public class AutoNumberService : Service<AutoNumber>,IAutoNumberService
    {
        private readonly IBankWithdrawalModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(IBankWithdrawalModuleRepositoryAsync<AutoNumber> autoNumberRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoNumberFlag(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(x => x.IsEditable).FirstOrDefault();
        }
    }
}
