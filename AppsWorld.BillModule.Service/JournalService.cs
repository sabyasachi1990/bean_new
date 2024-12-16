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
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IBillModuleRepositoryAsync<Journal> _receiptRepository;

        public JournalService(IBillModuleRepositoryAsync<Journal> receiptRepository)
            : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public Journal GetJournal(Guid id, long companyid)
        {
            return _receiptRepository.Query(a => a.Id == id && a.CompanyId == companyid).Select().FirstOrDefault();
        }

        public Journal GetJournals(Guid documentId, long companyId)
        {
            return _receiptRepository.Query(x => x.DocumentId == documentId && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<Journal> gstListOfJournal(long companyId, List<Guid> lstDocumentId)
        {
            return _receiptRepository.Query(a => a.CompanyId == companyId && lstDocumentId.Contains(a.DocumentId.Value)).Include(c => c.JournalDetails).Select().ToList();
        }
    }
}
