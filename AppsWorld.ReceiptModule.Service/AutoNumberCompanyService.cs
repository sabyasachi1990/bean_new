using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>, IAutoNumberCompanyService
    {
        private readonly IReceiptModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(IReceiptModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
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
