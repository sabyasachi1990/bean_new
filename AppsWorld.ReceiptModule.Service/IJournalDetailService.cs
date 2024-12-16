using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.ReceiptModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllJournal(Guid id);
        JournalDetail GetDetail(Guid id);
    }
}
