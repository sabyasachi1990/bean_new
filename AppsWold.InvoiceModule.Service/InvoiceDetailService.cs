using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class InvoiceDetailService : Service<InvoiceDetail>, IInvoiceDetailService
    {

        private readonly IInvoiceModuleRepositoryAsync<InvoiceDetail> _invoiceModuleService;
        public InvoiceDetailService(IInvoiceModuleRepositoryAsync<InvoiceDetail> invoiceModuleService)
            : base(invoiceModuleService)
        {
            _invoiceModuleService = invoiceModuleService;
        }

        public ICollection<InvoiceDetail> GetById(Guid Id)
        {
            return _invoiceModuleService.Query(a => a.InvoiceId == Id).Select().OrderBy(c => c.RecOrder).ToList();
        }
        public List<InvoiceDetail> GetInvoiceDetailById(Guid Id)
        {
            return _invoiceModuleService.Query(x => x.InvoiceId == Id).Select().ToList();
        }
   
        public InvoiceDetail GetAllInvoiceIdAndId(Guid invoiceId, Guid invoiceDetalId)
        {            
            return _invoiceModuleService.Query(a => a.InvoiceId == invoiceId && a.Id == invoiceDetalId).Select().FirstOrDefault();
        }
        


    }
}
