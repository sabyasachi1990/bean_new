using AppsWorld.CommonModule.Infra;
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
    public class ReceiptDetailService : Service<ReceiptDetail>, IReceiptDetailService
    {
        private readonly IInvoiceModuleRepositoryAsync<ReceiptDetail> _receiptDetailRepository;
        private readonly IInvoiceModuleRepositoryAsync<PaymentDetailCompact> _paymentDetailRepository;
        public ReceiptDetailService(IInvoiceModuleRepositoryAsync<ReceiptDetail> receiptDetailRepository, IInvoiceModuleRepositoryAsync<PaymentDetailCompact> paymentDetailRepository)
            : base(receiptDetailRepository)
        {
            this._receiptDetailRepository = receiptDetailRepository;
            this._paymentDetailRepository = paymentDetailRepository;
        }
        public List<ReceiptDetail> lstDetails(Guid DocumentId)
        {
            return _receiptDetailRepository.Query(c => c.DocumentId == DocumentId && c.ReceiptAmount > 0).Select().ToList();
        }
        public List<ReceiptDetail> GetAllReceiptsByDocumentId(Guid DocumentId)
        {
            return _receiptDetailRepository.Query(c => c.DocumentId == DocumentId && c.ReceiptAmount > 0).Include(a => a.Receipts).Select().ToList();
        }


        ////For Payment

        public List<PaymentDetailCompact> GetById(Guid bilid)
        {
            return _paymentDetailRepository.Query(a => a.DocumentId == bilid && a.PaymentAmount > 0 && a.DocumentState != "Void").Include(x => x.Payment).Select().ToList();
        }
    }
}
