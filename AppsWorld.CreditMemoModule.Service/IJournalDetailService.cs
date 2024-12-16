using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CreditMemoModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllJournal(Guid id);
        JournalDetail GetDetail(Guid id);
        JournalDetail GetDetailByJournalId(Guid journalId);
    }
}
