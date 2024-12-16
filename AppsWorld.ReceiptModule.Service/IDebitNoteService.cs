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
    public interface IDebitNoteService : IService<DebitNote>
    {
        DebitNote GetDebitNote(Guid id, string docCurrency);

        DebitNote GetDebitNotes(Guid id, string docCurrency);
        List<DebitNote> GetDebitNoteByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);
        List<string> GetByEntityId(Guid entityId, long companyId);

        DebitNote getDebitNoteById(Guid id, long companyId);
        DebitNote GetDebitNoteById(Guid id);
        DebitNote getDebitNoteeById(Guid id, long companyId, string currency, Guid entityId);
        List<DebitNote> GetDebitNoteByEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate);
        List<DebitNote> GetListOfDebitNote(long companyid, List<Guid> lstDocumentId);
        IQueryable<DebitNote> GetListOfDebitNoteNew(long companyid, List<Guid> lstDocumentId);
        List<string> GetAllDNByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetByIdState(Guid entityId, long companyId);
        List<string> GetAllDNByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<DebitNote> GetAllDebitNoteById(List<Guid> ids, long companyId, string currency, Guid entityId);
        List<BillCompact> GetBillByEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate);
        List<BillCompact> GetBillByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);
        List<BillCompact> GetAllBillsById(List<Guid> ids, long companyId, string currency, Guid entityId);
        List<BillCompact> GetAllBillsByDocId(List<Guid> ids, long companyId);
        IQueryable<BillCompact> GetAllBillsByDocIdNew(List<Guid> ids, long companyId);
        decimal? GetBillExchangeRate(Guid docId, long companyId);
        List<CreditMemoCompact> GetCMEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate);
        List<CreditMemoCompact> GetCMByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);
        List<CreditMemoCompact> GetAllCreditMemoById(List<Guid> ids, long companyId, string currency, Guid entityId);
        List<CreditMemoCompact> GetAllCMByDocId(List<Guid> ids, long companyId);
        IQueryable<CreditMemoCompact> GetAllCMByDocIdNew(List<Guid> ids, long companyId);
        void Update(BillCompact bill);
        decimal? GetMemoExchangeRate(Guid docId, long companyId);
        void Update(CreditMemoCompact creditMemo);
    }
}
