using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.InvoiceModule.Entities;
namespace AppsWorld.InvoiceModule.Service
{
    public class AutoNumberService : Service<AutoNumber>,IAutoNumberService
    {
        private readonly IInvoiceModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(IInvoiceModuleRepositoryAsync<AutoNumber> autoNumberRepository)
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
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(x=>x.IsEditable).FirstOrDefault();
        }
    }
}
