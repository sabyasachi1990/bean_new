using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CashSalesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>, IAutoNumberCompanyService
    {
        private readonly ICashSalesModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(ICashSalesModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
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
