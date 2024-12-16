using AppsWorld.PaymentModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public interface IDebitNoteCompactService : IService<DebitNoteCompact>
    {
        List<DebitNoteCompact> GetListOfDebitNotes(long companyId, string doctype, Guid entityId, List<Guid> documentIds);
        List<DebitNoteCompact> GetListOfDebitNoteWithOutInter(long companyId, long serviceCompanyId, Guid entityId, string currency, DateTime? docDate);
        List<DebitNoteCompact> GetListOfDebitNoteWithInter(long companyId, Guid entityId, string currency, DateTime? docDate);
        List<DebitNoteCompact> GetListOfDebitNotesByDocIds(long companyId, List<Guid> ids);
        List<DebitNoteCompact> GetlistDNByDocIds(List<Guid> docIds, string docCurrency, Guid entityId, long companyId);
        List<DebitNoteCompact> GetListOfDebitNote(long companyid, List<Guid> lstDocumentId);
        decimal? GetDebitNoteCompact(long companyId, Guid documentId);
        List<string> GetByEntityId(Guid entityId, long companyId);
        List<string> GetByIdState(Guid entityId, long companyId);
        List<string> GetAllDNByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllDNByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
    }
}
