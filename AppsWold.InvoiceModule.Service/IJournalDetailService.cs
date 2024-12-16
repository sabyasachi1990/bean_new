using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.InvoiceModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllJournal(Guid id);
        JournalDetail GetDetail(Guid id);
        List<JournalDetail> ListJDetail(Guid masterid);
        JournalDetail GetDetailByJournalId(Guid journalId);
        List<JournalDetail> AllJDetail(Guid journalId);
    }
}
