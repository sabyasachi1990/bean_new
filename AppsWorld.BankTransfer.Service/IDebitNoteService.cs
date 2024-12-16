using AppsWorld.BankTransferModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IDebitNoteService : IService<DebitNote>
    {
        List<DebitNote> GetListOfICDNBySEIdandEntId(long companyId, List<long?> lstServiceEntityIds, List<Guid> lstEntityIds, DateTime transferDate, string currency);
        List<DebitNote> GetListOfDNsByCompanyIdAndDocId(long companyId, List<Guid> docIds);
    }
}
