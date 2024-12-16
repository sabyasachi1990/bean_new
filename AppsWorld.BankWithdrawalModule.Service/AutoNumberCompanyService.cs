using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>, IAutoNumberCompanyService
    {
        private readonly IBankWithdrawalModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(IBankWithdrawalModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
            : base(autoNumbeCompanyrepository)
        {
            _autoNumbeCompanyrepository = autoNumbeCompanyrepository;
        }

        public List<AutoNumberCompany> GetAutoNumberCompany(Guid AutoNumberId)
        {
            return _autoNumbeCompanyrepository.Query(a => a.AutonumberId == AutoNumberId).Select().ToList();
        }
    }
}
