using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public class JournalDetailService:Service<JournalDetail>,IJournalDetailService
    {
        private readonly IBillModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IBillModuleRepositoryAsync<JournalDetail> journalDetailRepository):base(journalDetailRepository)
        {
            this._journalDetailRepository = journalDetailRepository;
        }

        public List<JournalDetail> getListOfJDetails(Guid journalid)
        {
            return _journalDetailRepository.Query(a => a.JournalId == journalid).Select().ToList();
        }
    }
}
