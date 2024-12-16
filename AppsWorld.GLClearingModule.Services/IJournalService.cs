using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.GLClearingModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.GLClearingModule.Service
{
   public interface IJournalService:IService<Journal>
    {
        List<Journal> GetAllJournal(DateTime date, long coaId, long serviceCompanyId);
        Journal GetJournal(long companyId, Guid documentId);
        Journal GetJournalDetalByJournal(Guid? journalId);
    }
}
