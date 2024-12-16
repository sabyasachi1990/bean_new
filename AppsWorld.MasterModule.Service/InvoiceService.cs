using AppsWorld.CommonModule.Infra;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        private readonly IMasterModuleRepositoryAsync<Invoice> _invoiceRepository;
        public InvoiceService(IMasterModuleRepositoryAsync<Invoice> invoiceRepository)
           : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;

        }
        public Invoice GetbyIdAndCompanyId(Guid guid, long CompanyId)
        {
            return _invoiceRepository.Query(c => c.Id == guid && c.CompanyId == CompanyId).Include(c => c.BeanEntiity).Select().FirstOrDefault();
        }
        public List<Invoice> GetAllByEntityId(Guid id)
        {
            return _invoiceRepository.Query(c => c.EntityId == id && c.DocumentState != InvoiceStates.Void && c.DocType == "Invoice").Include(c => c.InvoiceDetails).Select().ToList();
        }
        public List<Invoice> GetAllCnByEntityId(Guid id)
        {
            return _invoiceRepository.Query(c => c.EntityId == id && c.DocumentState != InvoiceStates.Void && c.DocType == "Credit Note").Select().ToList();
        }
        public async Task<Invoice> GetALlinvocesByItems(Guid? DocumentId)
        {

            return await Task.Run(()=> _invoiceRepository.Query(c => c.Id == DocumentId).Include(s => s.InvoiceDetails).Select().FirstOrDefault());
        }
        public Guid? GetEntityIdById(Guid guid, long CompanyId)
        {
            return   _invoiceRepository.Query(c => c.Id == guid && c.CompanyId == CompanyId).Select(d => d.EntityId).FirstOrDefault();
        }
    }
}
