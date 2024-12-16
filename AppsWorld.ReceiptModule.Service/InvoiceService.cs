using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        private readonly IReceiptModuleRepositoryAsync<Invoice> _invoiceRepository;
        private readonly IReceiptModuleRepositoryAsync<InvoiceDetail> _invoiceDetailRepository;

        public InvoiceService(IReceiptModuleRepositoryAsync<Invoice> invoiceRepository, IReceiptModuleRepositoryAsync<InvoiceDetail> invoiceDetailRepository)
            : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
        }
        public Invoice GetInvoice(Guid id, string docType, string docCurrency)
        {
            return _invoiceRepository.Query(c => c.Id == id && c.DocType == docType && c.DocCurrency == docCurrency && c.InternalState == "Posted").Select().FirstOrDefault();
        }

        public Invoice GetInvoices(Guid id, string docType, string docCurrency)
        {
            return _invoiceRepository.Query(c => c.Id == id && c.DocType == docType && c.DocCurrency == docCurrency && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().FirstOrDefault();
        }
        public List<Invoice> GetInvoicesByEntity(string docType, long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate)
        {
            return
                _invoiceRepository.Query(
                    c =>
                        c.DocType == docType && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate &&
                        (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0 && c.InternalState == "Posted").Select().ToList();
        }
        public List<Invoice> GetInvoicesCNByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate)
        {
            if (doccurrency != null)
                return
                    _invoiceRepository.Query(
                        c =>
                            (c.DocType == "Invoice" || c.DocType == "Credit Note") && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate &&
                            (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0 && (c.InternalState == "Posted" || c.InternalState == null)).Select().ToList();
            else
                return
                _invoiceRepository.Query(
                    c =>
                        (c.DocType == "Invoice" || c.DocType == "Credit Note") && c.CompanyId == companyId && c.EntityId == entityId && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate &&
                        (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0 && (c.InternalState == "Posted" || c.InternalState == null)).Select().ToList();
        }
        public List<string> GetByEntityId(Guid entityId, long companyId)
        {
            return _invoiceRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByStateandEntity(Guid entityId, long companyId)
        {
            return _invoiceRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
        public Invoice GetInvoiceById(Guid id, long companyId)
        {
            return _invoiceRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();

        }
        public Invoice GetInvoiceById(Guid id)
        {
            return _invoiceRepository.Query(c => c.Id == id).Select().FirstOrDefault();

        }
        public Invoice GetInvoiceeById(Guid id, long companyId, string currency, Guid entityId)
        {
            return _invoiceRepository.Query(c => c.Id == id && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void)).Select().FirstOrDefault();
        }
        public List<Invoice> GetAllInvoiceByDocId(List<Guid> id, long companyId, string currency, Guid entityId)
        {
            return _invoiceRepository.Query(c => id.Contains(c.Id) && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void) && c.Nature != "Interco").Select().ToList();
        }
        public List<Invoice> GetInvoiceByIdAndDocType(string docType, long companyId, Guid entityId, string doccurrency, DateTime? docDate)
        {
            return
                _invoiceRepository.Query(
                    c =>
                        c.DocType == docType && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.DocDate <= docDate &&
                        (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
        }
        public List<Invoice> GetInvoiceCNByIdAndDocType(long companyId, Guid entityId, string doccurrency, DateTime? docDate)
        {
            if (doccurrency != null)
                return
                    _invoiceRepository.Query(
                        c =>
                            (c.DocType == "Invoice" || c.DocType == "Credit Note") && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.DocDate <= docDate &&
                           (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0 && (c.InternalState == "Posted" || c.InternalState == null)).Select().ToList();
            else
                return
               _invoiceRepository.Query(
                   c =>
                       (c.DocType == "Invoice" || c.DocType == "Credit Note") && c.CompanyId == companyId && c.EntityId == entityId && c.DocDate <= docDate &&
                      (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0 && (c.InternalState == "Posted" || c.InternalState == null)).Select().ToList();
        }
        public Invoice GetInvoiceByDocument(Guid id)
        {
            return _invoiceRepository.Query(c => c.DocumentId == id && c.DocumentState != "Void").Select().FirstOrDefault();

        }
        public Invoice GetInvoiceByDocumentByVoid(Guid id)
        {
            return _invoiceRepository.Query(c => c.DocumentId == id).Select().FirstOrDefault();

        }
        public Invoice GetInvoiceByDocumentByState(Guid id)
        {
            return _invoiceRepository.Query(c => c.DocumentId == id && c.DocumentState != "Void").Select().FirstOrDefault();

        }
        public Guid GetInvoiceByDetailid(Guid id)
        {
            return _invoiceDetailRepository.Query(c => c.InvoiceId == id).Select(s => s.Id).FirstOrDefault();

        }
        public List<Invoice> GetListOfInvoices(long companyId, List<Guid> documentIds)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && documentIds.Contains(a.Id)).Select().ToList();
        }
        public IQueryable<Invoice> GetListOfInvoicesNew(long companyId, List<Guid> documentIds)
        {
            return _invoiceRepository.Queryable().Where(a => a.CompanyId == companyId && documentIds.Contains(a.Id)).AsQueryable();
        }
        public List<string> GetAllInvoiceByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _invoiceRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid || c.DocumentState == InvoiceState.PartialApplied || c.DocumentState == InvoiceState.Not_Applied)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllInvoiceByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _invoiceRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocType == "Invoice" || c.DocType == "Credit Note") && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState != InvoiceState.Void || c.DocumentState != InvoiceState.Deleted)).Select(c => c.DocCurrency).ToList();
        }
    }
}
