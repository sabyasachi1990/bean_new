using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        private readonly IInvoiceComptModuleRepositoryAsync<Invoice> _invoiceRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<InvoiceNoteCompact> _invoiceNoteRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<InvoiceDetail> _invoiceDetailRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<ReceiptCompact> _receiptRepository;

        public InvoiceService(IInvoiceComptModuleRepositoryAsync<Invoice> invoiceRepository, IInvoiceComptModuleRepositoryAsync<InvoiceNoteCompact> invoiceNoteRepository, IInvoiceComptModuleRepositoryAsync<InvoiceDetail> invoiceDetailRepository, IInvoiceComptModuleRepositoryAsync<ReceiptCompact> receiptRepository)
            : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._invoiceNoteRepository = invoiceNoteRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _receiptRepository = receiptRepository;
        }

        public Invoice GetRecInvoiceByIStateAndCId(string docType, string internalSate, long companyId)
        {
            return _invoiceRepository.Query(a => a.DocType == docType && a.InternalState == internalSate && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active && a.DocumentState != InvoiceStates.Void && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public ICollection<InvoiceNoteCompact> GetInvoiceByid(Guid Id)
        {
            return _invoiceNoteRepository.Query(a => a.InvoiceId == Id).Select().ToList();
        }
        public Invoice GetAllIvoiceByCidAndDocSubtype(string docType, long companyId, string docSubType)
        {
            return _invoiceRepository.Query(a => a.DocType == docType && a.CompanyId == companyId && a.DocSubType == docSubType && a.InternalState == "Posted" && a.DocumentState != InvoiceStates.Void && a.Status == RecordStatusEnum.Active && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public Invoice GetAllInvovoice(string strNewDocNo, string docType, long CompanyId)
        {
            return _invoiceRepository.Query(a => a.DocNo == strNewDocNo && a.DocType == docType && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public Invoice GetAllInvoiceByDoctypeAndCompanyId(string docType, long companyId)
        {
            return _invoiceRepository.Query(a => a.DocType == docType && a.CompanyId == companyId && a.DocumentState != InvoiceStates.Void && a.Status == RecordStatusEnum.Active && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public DateTime? GetByCompanyId(long companyId, string docType)
        {
            return _invoiceRepository.Query(e => e.CompanyId == companyId && e.DocType == docType && e.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).Select(e => e.CreatedDate).FirstOrDefault();
        }
        public Invoice GetCompanyAndId(long companyId, Guid id)
        {
            return _invoiceRepository.Query(e => e.CompanyId == companyId && e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(a => a.InvoiceNotes).Select().FirstOrDefault();
        }
        public Invoice GetAllInvoiceLu(long companyId, Guid Id)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.Id == Id && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Include(c => c.InvoiceDetails).Select().FirstOrDefault();
        }
        public bool GetRecurringDocNo(long companyId, Guid id, string internalState, string docNo)
        {
            return _invoiceRepository.Query(a => a.Id != id && a.CompanyId == companyId && a.InternalState == internalState && a.DocNo == docNo && a.Status == RecordStatusEnum.Active).Select().Any();
        }
        public Invoice GetInvoiceByIdAndDocumentId(Guid documentId, long companyId)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.IsWorkFlowInvoice == true).Include(a => a.InvoiceDetails).Select().FirstOrDefault();
        }
        public Invoice GetAllInvoiceByIdDocType(Guid Id)
        {
            return _invoiceRepository.Query(e => e.Id == Id && e.DocType == DocTypeConstants.Invoice && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(c => c.InvoiceNotes).Select().FirstOrDefault();
        }
        public List<Invoice> GetDocNumber(long companyId, string docNo)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId & a.DocNo.StartsWith(docNo) && a.DocSubType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public List<Invoice> GetInvoiceNumber(long companyId, string invNumber)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.InvoiceNumber == invNumber && a.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public List<Invoice> GetCompanyIdAndDocType(long companyId)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public InvoiceDetail GetAllInvoiceIdAndId(Guid invoiceId, Guid invoiceDetalId)
        {
            return _invoiceDetailRepository.Query(a => a.InvoiceId == invoiceId && a.Id == invoiceDetalId).Select().FirstOrDefault();
        }
        public string GetReceiptDocNo(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).Select(c => c.DocNo).FirstOrDefault();
        }
        public ReceiptCompact GetDocNo(string docNo, long companyId)
        {
            return _receiptRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public DateTime? GetReceiptsDate(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).Select(c => c.CreatedDate).FirstOrDefault();
        }
    }
}
