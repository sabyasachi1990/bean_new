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
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IReceiptModuleRepositoryAsync<Journal> _receiptRepository;

        public JournalService(IReceiptModuleRepositoryAsync<Journal> receiptRepository)
            : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
        public Journal GetJournals(Guid documentId, long companyId)
        {
            return _receiptRepository.Query(x => x.DocumentId == documentId && x.CompanyId == companyId).Include(c => c.JournalDetails).Select().FirstOrDefault();
        }
        public List<Journal> GetLstJournal(long companyId, Guid documentId)
        {
            return _receiptRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId).Select().ToList();
        }
    }
}
