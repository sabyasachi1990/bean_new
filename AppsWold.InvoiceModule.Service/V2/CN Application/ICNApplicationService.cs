using AppsWorld.InvoiceModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface ICNApplicationService : IService<CreditNoteApplication>
    {
        CreditNoteApplication GetAllCreditNote(Guid creditNoteId, Guid cnApplicationId, long companyId);
        CreditNoteApplication GetCreditNoteByCompanyId(long companyId);
        CreditNoteApplication GetCreditNoteById(Guid id);
        List<CreditNoteApplicationDetail> GetCreditNoteDetail(Guid CreditNoteApplicationId);
        CreditNoteApplication GetCreditNoteByIds(Guid Id);
        void Insert(CreditNoteApplicationDetail detail);
        void Update(CreditNoteApplicationDetail detail);
        void UpdateDebitNote(DebitNoteCompact debitNote);
        void UpdateInvoice(InvoiceCompact invoice);
        InvoiceCompact GetCreditNoteByCompanyIdAndId(long companyid, Guid id);
        List<InvoiceCompact> GetAllDDByInvoiceId(List<Guid> Ids);
        List<InvoiceCompact> GetAllCreditNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date);
        InvoiceCompact GetinvoiceById(Guid Id);
        Dictionary<Guid, string> GetTaggedInvoicesByCustomerAndCurrency(Guid customerId, string currency, long companyId);
        string GetCNNextNo(Guid id);
        List<InvoiceCompact> GetAllInvoiceByEntityId(long companyId, Guid EntityId, string DocCurrency, DateTime date);
        List<DebitNoteCompact> GetDDByDebitNoteId(List<Guid> Ids);
        List<DebitNoteCompact> GetAllDebitNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docdate);
        Dictionary<Guid, string> GetTaggedDebitNotesByCustomerAndCurrency(Guid customerId, string currency, long companyId);
        DebitNoteCompact GetDebitNoteById(Guid Id);
        List<DebitNoteCompact> GetAllDebitNoteByEntityId(long companyId, Guid EntityId, string DocCurrency, DateTime docdate);
        bool IsVoid(long companyId, Guid id);
        List<DebitNoteCompact> GetAllDebitNotesById(long companyId, Guid entityId, string docCurrency, long value, DateTime applicationDate);
        CreditNoteApplication GetAllCreditNoteApplication(Guid cnApplicationId, long companyId);
    }
}
