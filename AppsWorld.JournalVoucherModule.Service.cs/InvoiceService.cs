using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using FrameWork;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class InvoiceService : Service<Invoice>,IInvoiceService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Invoice> _invoiceRepository;

        public InvoiceService(IJournalVoucherModuleRepositoryAsync<Invoice> invoiceRepository)
            : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public Invoice GetInvoiceDetail(Guid invoiceId)
        {
            return _invoiceRepository.Query(x => x.Id == invoiceId).Select().SingleOrDefault();
        }
    }
}
