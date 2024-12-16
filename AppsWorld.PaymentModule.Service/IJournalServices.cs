using AppsWorld.PaymentModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public interface IJournalServices:IService<Journal>
    {
        Journal GetJournals(Guid documentId, long companyId);
        List<Journal> GetAllJournalsByDocId(List<Guid> journalIds, long companyId);
    }
}
