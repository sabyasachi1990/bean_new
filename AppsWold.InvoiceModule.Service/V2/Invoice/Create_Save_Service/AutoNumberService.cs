using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Service.Pattern;
using AppsWorld.InvoiceModule.Entities.V2;
namespace AppsWorld.InvoiceModule.Service.V2
{
    public class AutoNumberService : Service<AutoNumberCompact>, IAutoNumberService
    {
        private readonly IInvoiceComptModuleRepositoryAsync<AutoNumberCompact> _autoNumberepository;
        private readonly IInvoiceComptModuleRepositoryAsync<AutoNumberComptCompany> _autoNumbeCompanyRepository;

        public AutoNumberService(IInvoiceComptModuleRepositoryAsync<AutoNumberCompact> autoNumberRepository, IInvoiceComptModuleRepositoryAsync<AutoNumberComptCompany> autoNumbeCompanyRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
            this._autoNumbeCompanyRepository = autoNumbeCompanyRepository;
        }

        public AutoNumberCompact GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoNumberFlag(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(x => x.IsEditable).FirstOrDefault();
        }
        public List<AutoNumberComptCompany> GetAutoNumberCompany(Guid AutoNumberId)
        {
            return _autoNumbeCompanyRepository.Query(a => a.AutonumberId == AutoNumberId).Select().ToList();
        }
    }
}
