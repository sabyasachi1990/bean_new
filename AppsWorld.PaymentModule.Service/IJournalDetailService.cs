using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.PaymentModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.PaymentModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllJournal(Guid id);
        JournalDetail GetDetail(Guid id);
    }
}
