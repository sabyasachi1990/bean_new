using AppsWorld.ReceiptModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Service
{
    public interface IJournalService:IService<Journal>
    {
        Journal GetJournals(Guid documentId, long companyId);
        List<Journal> GetLstJournal(long companyId, Guid documentId);
    }
}
