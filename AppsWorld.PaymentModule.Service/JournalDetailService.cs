using AppsWorld.CommonModule.Infra;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IPaymentModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IPaymentModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
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
