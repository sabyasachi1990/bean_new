using AppsWorld.CommonModule.Infra;
using AppsWorld.GLClearingModule.Entities;
using AppsWorld.GLClearingModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppsWorld.GLClearingModule.Service
{
    public class JournalService:Service<Journal>,IJournalService
    {
        private readonly IClearingModuleRepositoryAsync<Journal> _journalRepository;
        public JournalService(IClearingModuleRepositoryAsync<Journal> journalRepository) : base(journalRepository)
        {
            _journalRepository = journalRepository;
        }
        public List<Journal> GetAllJournal(DateTime date,long coaId,long serviceCompanyId)
        {
            return _journalRepository.Query(x => x.DocDate <= date && x.COAId == coaId && x.ServiceCompanyId == serviceCompanyId &&(x.DocType==DocTypeConstants.Bill||x.DocType==DocTypeConstants.Deposit||x.DocType==DocTypeConstants.Withdrawal)).Select().ToList();
        }
        public Journal GetJournal(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId).Select().FirstOrDefault();
        }

        public Journal GetJournalDetalByJournal(Guid? journalId)
        {
            return _journalRepository.Queryable().Where(s => s.Id == journalId).FirstOrDefault();
        }
    }
}
