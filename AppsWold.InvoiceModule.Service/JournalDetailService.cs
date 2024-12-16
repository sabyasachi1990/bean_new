using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IInvoiceModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IInvoiceModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
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
        public List<JournalDetail> ListJDetail(Guid masterid)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == masterid).Select().ToList();
        }
        public JournalDetail GetDetailByJournalId(Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == journalId && x.DocumentDetailId == new Guid()).Select().FirstOrDefault();
        }
        public List<JournalDetail> AllJDetail(Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == journalId).Select().ToList();
        }
    }
}
