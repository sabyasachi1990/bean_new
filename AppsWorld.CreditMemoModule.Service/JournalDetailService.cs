using AppsWorld.CommonModule.Infra;
using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly ICreditMemoModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(ICreditMemoModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
        {
            _journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetAllJournal(Guid id)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == id).Select().ToList();
        }
        public JournalDetail GetDetail(Guid id)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == id && x.DocumentDetailId == new Guid()).Select().FirstOrDefault();
        }
        public JournalDetail GetDetailByJournalId(Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == journalId && x.DocumentDetailId == new Guid()).Select().FirstOrDefault();
        }
    }
}
