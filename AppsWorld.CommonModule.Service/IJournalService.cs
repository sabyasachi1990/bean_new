using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
    public interface IJournalService : IService<Journal>
    {
        Journal GetJournal(long companyId, Guid documentId);
        Journal GetJournals(Guid documentId, long companyId);
        List<Journal> GetLstJournal(long companyId, Guid documentId);
    }
}
