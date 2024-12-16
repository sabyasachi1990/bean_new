using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;

namespace AppsWorld.DebitNoteModule.Service
{
    public class InvoiceDetailService : Service<InvoiceDetail>, IInvoiceDetailService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<InvoiceDetail> _invoiceDetailRepository;
        public InvoiceDetailService(IDebitNoteMoluleRepositoryAsync<InvoiceDetail> invoiceDetailRepository)
            : base(invoiceDetailRepository)
        {
            _invoiceDetailRepository = invoiceDetailRepository;
        }
        public InvoiceDetail GetInvoiceDetail(Guid id)
        {
            return _invoiceDetailRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
