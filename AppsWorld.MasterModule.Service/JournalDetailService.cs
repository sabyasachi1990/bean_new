using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IMasterModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IMasterModuleRepositoryAsync<JournalDetail> journalDetailRepository)
            : base(journalDetailRepository)
        {
            this._journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetAllDetailByCOAId(long coaId)
        {
            return _journalDetailRepository.Query(x => x.COAId == coaId).Select().ToList();
        }
        public bool? GetAllByCoaId(List<long> coaId)
        {
            return _journalDetailRepository.Query(a => coaId.Contains(a.COAId)).Select().Any();
        }
        public bool? GetAllJDetailByCid(long coaId)
        {
            return _journalDetailRepository.Query(x => x.COAId == coaId).Select().Any();
        }
    }
}
