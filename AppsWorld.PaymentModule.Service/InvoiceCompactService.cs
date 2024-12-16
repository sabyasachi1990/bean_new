using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Infra;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public class InvoiceCompactService : Service<InvoiceCompact>, IInvoiceCompactService
    {
        private readonly IPaymentModuleRepositoryAsync<InvoiceCompact> _invoiceCompactRepository;
        private readonly IPaymentModuleRepositoryAsync<CreditNoteApplicationCompact> _creditNoteAppRepository;
        public InvoiceCompactService(IPaymentModuleRepositoryAsync<InvoiceCompact> invoiceCompactRepository, IPaymentModuleRepositoryAsync<CreditNoteApplicationCompact> creditNoteAppRepository) : base(invoiceCompactRepository)
        {
            this._invoiceCompactRepository = invoiceCompactRepository;
            this._creditNoteAppRepository = creditNoteAppRepository;
        }

        public List<InvoiceCompact> GetListOfInvoiceAndCN(long companyId, List<Guid> lstDocumentIds, Guid entityId, string docCurrency)
        {
            return _invoiceCompactRepository.Query(a => lstDocumentIds.Contains(a.Id) && a.CompanyId == companyId && a.EntityId == entityId && a.DocCurrency == docCurrency).Select().ToList();
        }
        public List<InvoiceCompact> GetListOfInvoiceAndCNWithOutInter(long companyId, long serviceCompanyId, Guid entityId, string currency, DateTime? docDate)
        {
            if (currency != null)
                return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.EntityId == entityId && a.DocCurrency == currency && (a.DocType == "Invoice" || a.DocType == "Credit Note") && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid" && a.DocumentState != "Fully Applied") && a.GrandTotal != 0 && (a.InternalState == "Posted" || a.InternalState == null)).Select().ToList();
            else
                return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.EntityId == entityId && (a.DocType == "Invoice" || a.DocType == "Credit Note") && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid" && a.DocumentState != "Fully Applied") && a.GrandTotal != 0 && (a.InternalState == "Posted" || a.InternalState == null)).Select().ToList();
        }
        public List<InvoiceCompact> GetListOfInvoiceAndCNWithInter(long companyId, Guid entityId, string currency, DateTime? docDate)
        {
            if (currency != null)
                return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && a.EntityId == entityId && a.DocCurrency == currency && (a.DocType == "Invoice" || a.DocType == "Credit Note") && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid" && a.DocumentState != "Fully Applied") && a.GrandTotal != 0 && (a.InternalState == "Posted" || a.InternalState == null)).Select().ToList();
            else
                return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && a.EntityId == entityId && (a.DocType == "Invoice" || a.DocType == "Credit Note") && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid" && a.DocumentState != "Fully Applied") && a.GrandTotal != 0 && (a.InternalState == "Posted" || a.InternalState == null)).Select().ToList();
        }

        public List<InvoiceCompact> GetListOfInvoiceAndCNs(long companyId, List<Guid> ids)
        {
            return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && ids.Contains(a.Id) && (a.DocType == "Invoice" || a.DocType == "Credit Note")).Select().ToList();
        }

        public List<Guid?> GetListOfCreditNoteApps(long companyId, List<Guid> docIds)
        {
            return _creditNoteAppRepository.Query(a => a.CompanyId == companyId && docIds.Contains(a.Id)).Select(a => a.DocumentId).ToList();
        }

        public List<InvoiceCompact> GetlistOfInvAndCNByDocIds(List<Guid> docIds, string docCurrency, Guid entityId, long companyId)
        {
            return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && a.DocCurrency == docCurrency && a.EntityId == entityId && docIds.Contains(a.Id)).Select().ToList();
        }
        public List<CreditNoteApplicationCompact> GetListOfCNAppsByCNId(List<Guid> docIds, long companyId)
        {
            return _creditNoteAppRepository.Query(a => a.CompanyId == companyId && docIds.Contains(a.InvoiceId) && a.DocumentId != null && a.Status == Framework.RecordStatusEnum.Active).Select().ToList();
        }
        public List<InvoiceCompact> GetListOfInvoices(long companyId, List<Guid> documentIds)
        {
            return _invoiceCompactRepository.Query(a => a.CompanyId == companyId && documentIds.Contains(a.Id)).Select().ToList();
        }
        public CreditNoteApplicationCompact GetCNApplicationByDocId(Guid detailId)
        {
            return _creditNoteAppRepository.Query(c => c.DocumentId == detailId).Select().FirstOrDefault();
        }

        public decimal? GetInvoiceAndCNByDocId(long companyId, Guid documentId)
        {
            return _invoiceCompactRepository.Query(a => a.Id == documentId && a.CompanyId == companyId && (a.DocType == "Invoice" || a.DocType == "Credit Note")).Select(a => a.ExchangeRate).FirstOrDefault();
        }
        public List<string> GetByEntityId(Guid entityId, long companyId)
        {
            return _invoiceCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == CreditMemoState.Not_Applied || c.DocumentState == CreditMemoState.Partial_Applied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByStateandEntity(Guid entityId, long companyId)
        {
            return _invoiceCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllInvoiceByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _invoiceCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == CreditMemoState.Not_Applied || c.DocumentState == CreditMemoState.Partial_Applied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllInvoiceByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _invoiceCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
    }
}
