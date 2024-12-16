using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IJournalService : IService<Journal>
    {
        Journal GetJournal(long companyId, Guid documentId);
        Journal GetJournals(Guid documentId, long companyId);
        List<Journal> GetLstJournal(long companyId, Guid documentId);
        List<Journal> GetAllJournalbyServiceCompanyId(long serviceCompanyId, long? companyId);
        Journal GetDoubtfulDebtRecord(long companyId, Guid docId);
        Journal GetJournalByDocId(long companyId, Guid documentId);
        List<Journal> GetListOfJournalByDocId(List<Guid> lstDocumentId, long companyId);
    }
}
