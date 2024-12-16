using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class AutoNumberCompanyService : Service<AutoNumberCompany>,IAutoNumberCompanyService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<AutoNumberCompany> _autoNumbeCompanyrepository;

        public AutoNumberCompanyService(IJournalVoucherModuleRepositoryAsync<AutoNumberCompany> autoNumbeCompanyrepository)
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
