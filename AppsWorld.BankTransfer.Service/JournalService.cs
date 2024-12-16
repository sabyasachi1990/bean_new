using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IBankTransferModuleRepositoryAsync<Journal> _journalRepository;
        public JournalService(IBankTransferModuleRepositoryAsync<Journal> journalRepository) : base(journalRepository)
        {
            this._journalRepository = journalRepository;
        }
        public List<Journal> GetListOfJournalBYCompIdandDocId(long companyId, List<Guid> lstOfDocIds)
        {
            return _journalRepository.Query(a => a.CompanyId == companyId && lstOfDocIds.Contains(a.DocumentId.Value)).Select().ToList();
        }
    }
}
