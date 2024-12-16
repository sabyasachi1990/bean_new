using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Models;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IJournalService : IService<Journal>
    {
        List<Journal> GetaAllJournalParkedById(long companyId);

        List<Journal> GetAllRecurringsById(long companyId);
        List<Journal> GetAllAutoReversalByid(long companyId);
        List<Journal> GetJournalByCompanyId(long companyId, string documentState, string docSubType);

        List<Journal> GetAllRevalJournalByDocId(Guid? documentId, long companyId);
        List<Journal> GetAllJVPostedByCompanyId(long companyId);
        List<Journal> GetAllJVVoidByCompanyId(long companyId);
        Task<Journal> GetCompanyId(long companyId);
        Journal CreateJournalDocNo(long companyId);
        List<Journal> GetAllJournals();
        Task<Journal> GetJournalDateBySubType(long companyId, string docSubType);
        Journal GetByAllJournal(Guid id);
        Journal GetAllReversal(Guid id);
        Journal GetJournalById(Guid id, long companyId);
        Journal GetJournalByReverseParentId(Guid id, long companyId);
        Journal GetDocNo(string docNo, long companyId);
        List<Journal> GetJVByCompanyId(Guid id, long companyId);
        Journal GetByDocTypeId(Guid id, string DocType, string DocNo, long companyId, string documentState);
        IQueryable<JournalModelK> GetaAllJournalPostedById(long companyId);

        IQueryable<JournalModelParkedK> GetaAllJVParkedById(string username, long companyId);
        IQueryable<JournalModelRecurringK> GetaAllJVRecurringById(long companyId);

        Journal GetBydocumentId(Guid documentId, long companyId);
        Journal GetByReceiptdocumentId(Guid documentId, long companyId);
        Task<bool?> GetIsLockedByReverseParentId(Guid id, long companyId);
        string GetJournalRefNo(Guid id, long companyId);
        List<Journal> GetAllRecurringJournal(long companyId, Guid journalId);

        bool? GetNosupporting(long companyId);

        bool? Getallowablenonallowable(long companyId);
        Journal GetJournalByid(Guid id, long companyId);
        List<Journal> GetListBydocumentId(Guid? documentId, long companyId);
        Journal GetByTransferdocumentId(Guid documentId, long companyId, bool? isWithdrawal);
        Journal CheckOpeningbalance(string docType, long companyId, long? serviceCompanyId);
        List<Journal> GetJournalReferenceNo(Guid? id, long companyId);
        IQueryable<JournalModelParkedK> GetaAllVoidedK(long companyId);

        Journal GetJournalByCIDandDocSubType(long companyId, string docType, string docSubtype, string docNo);

        Journal GetJournalByCIDandDocSubTypeAndId(Guid id, long companyId, string docType, string docSubtype, string docNo);

        List<Journal> GetJournalByCompanyIdAndDocSubType(long companyId, string docSubType);

        Journal GetJournalByIdandRecurringId(long companyId, Guid recurringId);
        Journal GetRecurringJournalByReccuringId(long companyId, Guid recurringId);
        List<string> GetDocNoByCompanyId(long companyId, string documentSatate, string docNo);
        List<string> GetPostedJournalDocNo(long companyId, string documentState);
        IQueryable<JournalModelK> GetAllPostedJournal(long companyId, Guid? recurringJournalId);

        List<Journal> GetJournalByCidandRecurringId(long companyId, Guid recurringId);

        Journal GetAllJournalById(Guid id, long companyId);
        List<Journal> GetAllPostedJournalByCid(long companyId);
        Journal GetAllJournalByRecurringId(Guid id, long comapnyId);
        List<string> GetPostedJournalsDocNo(long companyId, string documentState);

        Journal CreateJournalDocNo(long companyId, string documentState);
        Journal GetDocNoByDocumentSate(string docNo, long companyId, string documentState);

        Journal GetDocumentByDIdandCompanyId(long companyId, Guid documentId, string docType);
        IQueryable<InvoiceAuditTrailModel> GetDeletedAuditTrail(Guid journalId);

        List<Guid> GetJournalsIdByDocumentId(long companyId, Guid documentId, string docType);
        List<Journal> GetLstOfJournals(List<Guid> journalIds);
        Journal GetDocumentIdByDIDAndDocSubType(long companyId, Guid documentId, string docSubType);//based on docSubType

        List<Journal> GetAllPostedJournals(long companyId);
        List<Journal> GetLstOFJournalsByDocumentId(Guid documentId, string docSubType, long companyId);
        Task<DateTime> GetReversalDate(Guid? reversalId, long companyId);

        Task<Journal> GetJournalByIdAndCid(Guid id, long companyId);
        Journal GetRecurringJournal(long companyId, Guid? recurringId);

        Task<Journal> GetLastJournal(long companyId, string documentState);

        IQueryable<JournalModelRecurringK> GetAllRecurringsByCompanyId(long companyId,string username);


        //new call for journal Kendo
        Task<IQueryable<JournalModelK>> NewGetAllPostedjournals(long companyId, string userName);
        bool IsVoid(long companyId, Guid id);

        Task<Journal> GetAllJournalByIdAsync(Guid id, long companyId);

        

    }
}
