using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>, IAutoNumberCompanyService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(IOpeningBalancesModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
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
