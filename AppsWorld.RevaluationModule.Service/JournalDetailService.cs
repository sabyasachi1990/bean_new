using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IRevaluationModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IRevaluationModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
        {
            this._journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetPostingJournalDetail(DateTime dateTime)
        {
            return _journalDetailRepository.Query(x => x.DueDate == dateTime).Select().ToList();
        }

        public List<JournalDetail> GetAllOnlyJournalDetails(Guid jorunalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == jorunalId && x.DocumentDetailId != new Guid("00000000-0000-0000-0000-000000000000")).Select().ToList();
        }
        public JournalDetail GetAllJournalDetails(Guid jorunalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == jorunalId && x.DocumentDetailId == new Guid()).Select().FirstOrDefault();
        }
    }
}
