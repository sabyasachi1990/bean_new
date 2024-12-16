using AppsWorld.InvoiceModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface IInvoiceService : IService<Invoice>
    {
        Invoice GetRecInvoiceByIStateAndCId(string docType, string internalSate, long companyId);
        Invoice GetAllInvovoice(string strNewDocNo, string docType, long CompanyId);
        Invoice GetAllIvoiceByCidAndDocSubtype(string docType, long companyId, string docSubType);
        Invoice GetAllInvoiceByDoctypeAndCompanyId(string docType, long companyId);
        DateTime? GetByCompanyId(long companyId, string docType);
        Invoice GetCompanyAndId(long companyId, Guid id);
        ICollection<InvoiceNoteCompact> GetInvoiceByid(Guid Id);
        Invoice GetAllInvoiceLu(long companyId, Guid Id);
        bool GetRecurringDocNo(long companyId, Guid id, string internalState, string docNo);
        Invoice GetInvoiceByIdAndDocumentId(Guid documentId, long companyId);
        Invoice GetAllInvoiceByIdDocType(Guid Id);
        List<Invoice> GetDocNumber(long companyId, string docNo);
        List<Invoice> GetInvoiceNumber(long companyId, string invNumber);
        List<Invoice> GetCompanyIdAndDocType(long companyId);
        InvoiceDetail GetAllInvoiceIdAndId(Guid invoiceId, Guid invoiceDetalId);
        string GetReceiptDocNo(long companyId);
        ReceiptCompact GetDocNo(string docNo, long companyId);
        DateTime? GetReceiptsDate(long companyId);
    }
}
