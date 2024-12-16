using AppsWorld.BillModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public interface IJournalService:IService<Journal>
    {
        Journal GetJournals(Guid documentId, long companyId);
        List<Journal> gstListOfJournal(long companyId, List<Guid> lstDocumentId);
        Journal GetJournal(Guid id, long companyid);
    }
}
