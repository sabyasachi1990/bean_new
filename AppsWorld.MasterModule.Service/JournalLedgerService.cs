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
    //public class JournalLedgerService : Service<JournalLedger>, IJournalLedgerService
    //{
        
    //    private readonly IMasterModuleRepositoryAsync<JournalLedger> _journalLedgerRepository;

    //    public JournalLedgerService(IMasterModuleRepositoryAsync<JournalLedger> journalLedgerRepository)
    //        : base(journalLedgerRepository)
    //    {
    //        this._journalLedgerRepository = journalLedgerRepository;
    //    }
    //    public bool IsItemTransactionPosted(Guid itemId, long companyId)
    //    {
    //        return _journalLedgerRepository.Query(a => a.ItemId == itemId && a.CompanyId == companyId).Select().Any();
    //    }

    //}
}
