using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Service;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IJournalVoucherModuleRepositoryAsync<JournalDetail> journalDetailRepository)
            : base(journalDetailRepository)
        {
            this._journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetAllJournalDetailsByid(Guid jorunalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == jorunalId && (x.IsTax == false || x.IsTax == null)).Select().ToList();
        }
        
        public List<JournalDetail> GetAllJournalDetailsByidForView(Guid jorunalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == jorunalId).Select().ToList();
        }
        public JournalDetail GetJournalDetailsById(Guid Id)
        {
            return _journalDetailRepository.Query(x => x.Id == Id).Select().FirstOrDefault();
        }
        public JournalDetail GetJDdetailJournalId(Guid Id, Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.Id == Id && x.JournalId == journalId).Select().FirstOrDefault();
        }
        public List<JournalDetail> GetJDdetailJournals(Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == journalId && x.IsTax != true).Select().ToList();
        }
        public List<JournalDetail> GetAllOnlyJournalDetails(Guid jorunalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == jorunalId && x.DocumentDetailId != new Guid("00000000-0000-0000-0000-000000000000")).Select().ToList();
        }
        public JournalDetail GetJournalDetailById(Guid journalId)
        {
            return _journalDetailRepository.Query(x => x.JournalId == journalId && x.DocumentDetailId == new Guid()).Select().FirstOrDefault();
        }
        //public List<JournalDetail> GetAllBTJournalDetails(string systemRefNo)
        //{
        //    return _journalDetailRepository.Query(x => x.SystemRefNo == systemRefNo).Select().ToList();
        //}
    }
}
