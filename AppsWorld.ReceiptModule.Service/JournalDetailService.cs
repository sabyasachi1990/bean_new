using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IReceiptModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IReceiptModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
        {
            _journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetAllJournal(Guid id)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == id).Select().ToList();
        }
        public JournalDetail GetDetail(Guid id)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == id&&x.DocumentDetailId==new Guid()).Select().FirstOrDefault();
        }
    }
}
