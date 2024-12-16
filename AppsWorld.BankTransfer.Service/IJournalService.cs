using AppsWorld.BankTransferModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IJournalService : IService<Journal>
    {
        List<Journal> GetListOfJournalBYCompIdandDocId(long companyId, List<Guid> lstOfDocIds);
    }
}
