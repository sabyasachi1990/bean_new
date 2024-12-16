using AppsWorld.PaymentModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public interface IInvoiceCompactService : IService<InvoiceCompact>
    {
        List<InvoiceCompact> GetListOfInvoiceAndCN(long companyId, List<Guid> lstDocumentIds, Guid entityId, string docCurrency);
        List<InvoiceCompact> GetListOfInvoiceAndCNWithOutInter(long companyId, long serviceCompanyId, Guid entityId, string currency, DateTime? docDate);
        List<InvoiceCompact> GetListOfInvoiceAndCNWithInter(long companyId, Guid entityId, string currency, DateTime? docDate);
        List<InvoiceCompact> GetListOfInvoiceAndCNs(long companyId, List<Guid> ids);
        List<Guid?> GetListOfCreditNoteApps(long companyId, List<Guid> docIds);
        List<InvoiceCompact> GetlistOfInvAndCNByDocIds(List<Guid> docIds, string docCurrency, Guid entityId, long companyId);
        List<CreditNoteApplicationCompact> GetListOfCNAppsByCNId(List<Guid> docIds, long companyId);
        List<InvoiceCompact> GetListOfInvoices(long companyId, List<Guid> documentIds);
        CreditNoteApplicationCompact GetCNApplicationByDocId(Guid detailId);
        decimal? GetInvoiceAndCNByDocId(long companyId, Guid documentId);
        List<string> GetByEntityId(Guid entityId, long companyId);
        List<string> GetByStateandEntity(Guid entityId, long companyId);
        List<string> GetAllInvoiceByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllInvoiceByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
    }
}
