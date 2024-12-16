using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service
{
    public class Invoiceservice : Service<Invoice>, IInvoiceService
    {
        private readonly ITemplateModuleRepositoryAsync<Invoice> _invoiceRepository;
        private readonly ITemplateModuleRepositoryAsync<TaxCode> _taxCodeRepository;
        private readonly ITemplateModuleRepositoryAsync<Journal> _journalRepository;

        public Invoiceservice(ITemplateModuleRepositoryAsync<Invoice> invoiceRepository, ITemplateModuleRepositoryAsync<TaxCode> taxCodeRepository, ITemplateModuleRepositoryAsync<Journal> journalRepository) : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _taxCodeRepository = taxCodeRepository;
            _journalRepository = journalRepository;
        }

        public List<Journal> GetJournal(Guid entityId)
        {
            List<Journal> journal = _journalRepository.Query(v => v.EntityId == entityId).Select().ToList();
            return journal;


        }

        public TaxCode GetTaxCode(long? taxId)
        {
            TaxCode taxCode = _taxCodeRepository.Query(v => v.Id == taxId).Select().FirstOrDefault();
            return taxCode;
        }
    }
}
