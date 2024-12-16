using AppsWorld.BankReconciliationModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public interface IJournalService : IService<Journal>
    {
        List<Journal> GetAllJournal(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate, bool isClearing, bool isBankReconcile);
        // List<Journal> GetAllJournalK(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate);

        Journal GetJournal(Guid id, long companyId);
        List<JournalDetail> GetAllJournalDetails(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate, bool isClearing, bool isBankReconcile, DateTime? lastReconDate);
        List<JournalDetail> GetAllCleraingJournalDetails(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate, bool isClearing);

        List<JournalDetail> GetlstJournalDetailByCoaId(long serviceCompanyId, long coaId, DateTime? reconciledDate, DateTime? lastReconciledDate);//modified by Lokanath for new Bank Reconciliation Condition
        List<Guid?> GetListJournalDetailIds(long serviceCompanyId, long coaId, DateTime? reconciledDate, DateTime? lastReconciledDate);

        Task<decimal?> GetGLBalance(long coaId, long companyId, long serviceCompanyId, DateTime reconciledDate);
        List<JournalDetail> GetListOfJournalDetail(long coaId, long serviceCompanyId);
        List<JournalDetail> GetListOfClearedDetail(long companyId, long serviceCompanyId, long coaId, DateTime? brcReocnDate);
        List<JournalDetail> lstJournalDetail(Guid? reconciliationId, long coaId, long serviceCompanyId);

        List<Journal> GetListOfJournalByJournalId(long companyId, List<Guid> lstJournalIds);

        //Journal Void checking
        bool IfAnyJournalVoid(long companyId, List<Guid?> lstDocumentIds);
    }
}
