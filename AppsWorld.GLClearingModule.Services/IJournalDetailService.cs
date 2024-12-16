using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.GLClearingModule.Entities;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;

namespace AppsWorld.GLClearingModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllJournalD(DateTime? fromDate, DateTime? toDate, long coaId, long serviceCompanyId, bool? IsClearingChecked);
        List<JournalDetail> GetJournalDetail(long coaId, long serviceCompanyId, DateTime? docDate, Guid? entityId);
        List<JournalDetail> GetAllJournal(Guid id);
        List<JournalDetail> GetAllDetail(List<Guid?> ids);
        List<JournalDetail> GetAllJournalD(DateTime? fromDate, DateTime? toDate, long coaId, long serviceCompanyId, Guid entityId, bool? IsClearingChecked);
        Guid GetJournalDetalByJournalId(Guid? documentId);
        JournalDetail GetJournalDetalByJournal(Guid? documentId);
        List<JournalDetail> GetAllJDByDocId(List<Guid?> lstDocIds);
        Dictionary<Guid, decimal?> GetAllJDByIds(List<Guid> lstIds);
    }
}
