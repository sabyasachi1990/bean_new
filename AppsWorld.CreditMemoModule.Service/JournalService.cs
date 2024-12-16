using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.RepositoryPattern;

namespace AppsWorld.CreditMemoModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly ICreditMemoModuleRepositoryAsync<Journal> _journalRepository;

        public JournalService(ICreditMemoModuleRepositoryAsync<Journal> journalRepository)
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
        public List<Journal> GetListOfJournalByDocId(List<Guid?> lstDocumentId, long companyId)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && lstDocumentId.Contains(a.DocumentId)).Select().ToList();
        }

    }
}
