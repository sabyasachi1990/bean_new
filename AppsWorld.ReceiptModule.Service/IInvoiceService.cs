using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface IInvoiceService : IService<Invoice>
    {
        Invoice GetInvoices(Guid id, string docType, string docCurrency);

        Invoice GetInvoice(Guid id, string docType, string docCurrency);
        List<Invoice> GetInvoicesByEntity(string docType, long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);

        List<string> GetByEntityId(Guid entityId, long companyId);

        Invoice GetInvoiceById(Guid id, long companyId);
        Invoice GetInvoiceById(Guid id);
        Invoice GetInvoiceeById(Guid id, long companyId, string currency, Guid entityId);

        List<Invoice> GetInvoiceByIdAndDocType(string docType, long companyId, Guid entityId, string doccurrency, DateTime? docDate);
        Invoice GetInvoiceByDocument(Guid id);
        Invoice GetInvoiceByDocumentByVoid(Guid id);
        Invoice GetInvoiceByDocumentByState(Guid id);
        Guid GetInvoiceByDetailid(Guid id);
        List<Invoice> GetListOfInvoices(long companyId, List<Guid> documentIds);
        IQueryable<Invoice> GetListOfInvoicesNew(long companyId, List<Guid> documentIds);
        List<string> GetAllInvoiceByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetByStateandEntity(Guid entityId, long companyId);
        List<string> GetAllInvoiceByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<Invoice> GetAllInvoiceByDocId(List<Guid> id, long companyId, string currency, Guid entityId);
        List<Invoice> GetInvoiceCNByIdAndDocType(long companyId, Guid entityId, string doccurrency, DateTime? docDate);
        List<Invoice> GetInvoicesCNByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);
    }
}
