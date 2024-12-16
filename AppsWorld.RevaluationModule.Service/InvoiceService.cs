using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.RevaluationModule.Service
{
    public class InvoiceService:Service<Invoice>,IInvoiceService
    {
        private readonly IRevaluationModuleRepositoryAsync<Invoice> _invoiceRepository;
        public InvoiceService(IRevaluationModuleRepositoryAsync<Invoice> invoiceRepository)
            : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public List<Invoice> lstInvoices(long companyId)
        {
            return _invoiceRepository.Query(c => c.CompanyId == companyId && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid)).Select().ToList();
        }
    }
}
