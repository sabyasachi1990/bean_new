using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>, IAutoNumberCompanyService
    {
        private readonly ICommonModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(ICommonModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
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
