using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;

namespace AppsWorld.InvoiceModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IInvoiceModuleRepositoryAsync<Journal> _journalRepository;

        public JournalService(IInvoiceModuleRepositoryAsync<Journal> journalRepository)
            : base(journalRepository)
        {
            _journalRepository = journalRepository;
        }
        public Journal GetJournal(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId).Select().FirstOrDefault();
        }
        public Journal GetJournals(Guid documentId, long companyId)
        {
            return _journalRepository.Query(x => x.DocumentId == documentId && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<Journal> GetLstJournal(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId).Select().ToList();
        }
        public List<Journal> GetAllJournalbyServiceCompanyId(long serviceCompanyId, long? companyId)
        {
            return _journalRepository.Query(x => x.ServiceCompanyId == serviceCompanyId && x.IsGstSettings == true && x.DocumentState == "Recurring" && x.CompanyId == companyId).Include(c => c.JournalDetails).Select().ToList();
        }
        public Journal GetDoubtfulDebtRecord(long companyId, Guid docId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == docId).Include(a => a.JournalDetails).Select().FirstOrDefault();
        }
        public Journal GetJournalByDocId(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId && x.DocumentState == "Reversed").Select().FirstOrDefault();
        }
        public List<Journal> GetListOfJournalByDocId(List<Guid> lstDocumentId, long companyId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && lstDocumentId.Contains(a.DocumentId.Value)).Select().ToList();
        }
    }
}
