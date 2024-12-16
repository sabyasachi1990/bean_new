using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class InvoiceDetailService:Service<InvoiceDetail>,IInvoiceDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<InvoiceDetail> _invoceDetailServiceRepository;
        public InvoiceDetailService(IJournalVoucherModuleRepositoryAsync<InvoiceDetail> invoceDetailServiceRepository)
            : base(invoceDetailServiceRepository)
        {
            _invoceDetailServiceRepository = invoceDetailServiceRepository;
        }
        public List<InvoiceDetail> GetAllInvoiceDeatil(Guid invoiceId)
        {
            return _invoceDetailServiceRepository.Query(x => x.InvoiceId == invoiceId).Select().ToList();
        }
    }
}
