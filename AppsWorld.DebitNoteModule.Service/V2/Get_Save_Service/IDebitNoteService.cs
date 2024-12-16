using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities.V2;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public interface IDebitNoteService : IService<DebitNote>
    {
        DebitNote GetDebitNoteById(Guid id, long companyId);
        DebitNote GetDebitNote(Guid id, long companyId);
        Dictionary<DateTime, DateTime?> GetDocDate(long companyId);
        DebitNote CreateDebitNoteForDocNo(long companyId);
        DebitNote GetDocNo(string docNo, long companyId);
        DebitNote GetDebitNoteDocNo(Guid id, string docNo, long companyId);
        List<DebitNote> GetAllDebitModel(long companyId);
        DebitNote GetDebittNote(Guid id);
        DateTime? GetDNLastPostedDate(long companyId);
        DebitNoteDetail GetDebitNoteDetail(Guid id, Guid debitNoteId);
        DateTime? GetInvoiceByCompany(long companyId, string docType);
        DateTime? GetDebitNoteCreatedDate(Guid id, long companyId);
        string GetCNDocDate(long companyId, string docType);
        string GetDuplicateInvoice(long companyId, string docType, string docNo);
    }
}
