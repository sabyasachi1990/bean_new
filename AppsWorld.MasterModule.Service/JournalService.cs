using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IMasterModuleRepositoryAsync<Journal> _journalRepository;
        private readonly IMasterModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalService(IMasterModuleRepositoryAsync<Journal> journalRepository, IMasterModuleRepositoryAsync<JournalDetail> journalDetailRepository)
            : base(journalRepository)
        {
            this._journalRepository = journalRepository;
            this._journalDetailRepository = journalDetailRepository;
        }
        public Journal GetByCompanyId(long CompanyId)
        {
            return _journalRepository.Queryable().Where(x => x.CompanyId == CompanyId).FirstOrDefault();
        }

        public DeleteChartofaccount GetJournalPosting(long companyId, long coaId)
        {
            // var result1 = _journalDetailRepository.Queryable().Where(s => s.COAId == coaId).FirstOrDefault();
            var result = (from j in _journalRepository.Queryable()
                          join jd in _journalDetailRepository.Queryable()
                          on j.Id equals jd.JournalId
                          where j.CompanyId == companyId && jd.COAId == coaId
                          select new DeleteChartofaccount()
                          {
                              CompanyId = j.CompanyId,
                              CoaId = jd.COAId
                          }).FirstOrDefault();
            return result;

        }

        public JournalDetail GetJournaldetailPosting(long companyId, long coaId)
        {
            var result1 = _journalDetailRepository.Queryable().Where(s => s.COAId == coaId).FirstOrDefault();
            return result1;

        }
    }
}
