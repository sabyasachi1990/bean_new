using AppsWorld.CommonModule.Infra;
using AppsWorld.GLClearingModule.Entities;
using AppsWorld.GLClearingModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.Infra;
using System.Data.Entity.Core.Objects;

namespace AppsWorld.GLClearingModule.Service
{
    public class JournalDetailService : Service<JournalDetail>, IJournalDetailService
    {
        private readonly IClearingModuleRepositoryAsync<JournalDetail> _journalDetailRepository;
        public JournalDetailService(IClearingModuleRepositoryAsync<JournalDetail> journalDetailRepository) : base(journalDetailRepository)
        {
            _journalDetailRepository = journalDetailRepository;
        }
        public List<JournalDetail> GetAllJournalD(DateTime? fromDate, DateTime? toDate, long coaId, long serviceCompanyId, bool? IsClearingChecked)
        {
            //List<JournalDetail> lstjournal = new List<JournalDetail>();
            //lstjournal = _journalDetailRepository.Query(x => x.DocDate <= date && x.COAId == coaId && x.ServiceCompanyId == serviceCompanyId && (x.ClearingStatus != ClearingState.Cleared && x.ClearingStatus == null) && (x.DocType == DocTypeConstants.Deposit || x.DocType == DocTypeConstants.Withdrawal)).Select().ToList();
            List<JournalDetail> lstbill = _journalDetailRepository.Query(x => x.COAId == coaId && ((fromDate != null && toDate != null) ? (x.DocDate >= fromDate && x.DocDate <= toDate) : (fromDate == null && toDate == null) ? true : fromDate != null ? (x.DocDate >= fromDate) : (x.DocDate <= toDate)) && x.ServiceCompanyId == serviceCompanyId && (IsClearingChecked != true ? (x.ClearingStatus == null && x.ClearingDate == null) : ((x.ClearingStatus == ClearingState.Cleared && x.ClearingDate != null) || (x.ClearingStatus != ClearingState.Cleared && x.ClearingDate == null))) && (x.IsTax == false || x.IsTax == null)).Select().ToList();
            //if (lstjournal.Count > 0)
            //{
            //    if (lstbill.Any())
            //    {
            //        var lst = lstjournal.Concat(lstbill);
            //        return lst.ToList();
            //    }
            //    else
            //        return lstjournal;
            //}
            //else
            //{
            return lstbill;
            //}
        }

        public List<JournalDetail> GetJournalDetail(long coaId, long serviceCompanyId, DateTime? docDate, Guid? entityId)
        {
            //List<JournalDetail> lstjournal = new List<JournalDetail>();
            //lstjournal = _journalDetailRepository.Query( x=> x.COAId == coaId && x.ServiceCompanyId == serviceCompanyId && (x.ClearingStatus != ClearingState.Cleared && x.ClearingStatus == null) && (x.DocType == DocTypeConstants.Deposit || x.DocType == DocTypeConstants.Withdrawal)).Select().ToList();
            List<JournalDetail> lstbill = _journalDetailRepository.Query(x => x.COAId == coaId && x.DocDate <= docDate && x.ServiceCompanyId == serviceCompanyId && x.EntityId == entityId && (x.ClearingStatus != ClearingState.Cleared && x.ClearingStatus == null) && (x.IsTax == false || x.IsTax == null)).Select().ToList();
            //if (lstjournal.Count > 0)
            //{
            //    if (lstbill.Any())
            //    {
            //        var lst = lstjournal.Concat(lstbill);
            //        return lst.ToList();
            //    }
            //    else
            //        return lstjournal;
            //}
            //else
            //{
            return lstbill;
            // }
            //return jd.Where(x => x.DocumentDetailId == new Guid("00000000-0000-0000-0000-000000000000")).ToList();
        }
        public List<JournalDetail> GetAllJournal(Guid id)
        {
            return _journalDetailRepository.Query(x => x.DocumentId == id).Select().ToList();
        }
        public List<JournalDetail> GetAllDetail(List<Guid?> ids)
        {
            return _journalDetailRepository.Query(x => ids.Contains(x.Id)).Select().ToList();
        }

        public List<JournalDetail> GetAllJournalD(DateTime? fromDate, DateTime? toDate, long coaId, long serviceCompanyId, Guid entityId, bool? IsClearingChecked)
        {

            List<JournalDetail> lstbill = _journalDetailRepository.Query(x => x.COAId == coaId && ((fromDate != null && toDate != null) ? ((EntityFunctions.TruncateTime(x.DocDate) >= EntityFunctions.TruncateTime(fromDate) && EntityFunctions.TruncateTime(x.DocDate) <= EntityFunctions.TruncateTime(toDate))) : (fromDate != null && toDate == null) ? (EntityFunctions.TruncateTime(x.DocDate) >= EntityFunctions.TruncateTime(fromDate)) : false) && x.ServiceCompanyId == serviceCompanyId && x.EntityId == entityId && (IsClearingChecked != true ? (x.ClearingStatus != ClearingState.Cleared && x.ClearingStatus == null) : (x.ClearingStatus == ClearingState.Cleared && x.ClearingDate != null)) && (x.IsTax == false || x.IsTax == null)).Select().ToList();
            return lstbill;
        }

        public Guid GetJournalDetalByJournalId(Guid? documentId)
        {
            return _journalDetailRepository.Queryable().Where(s => s.Id == documentId).Select(s => s.JournalId).FirstOrDefault();
        }
        public JournalDetail GetJournalDetalByJournal(Guid? documentId)
        {
            return _journalDetailRepository.Queryable().Where(s => s.Id == documentId).FirstOrDefault();
        }
        public List<JournalDetail> GetAllJDByDocId(List<Guid?> lstDocIds)
        {
            return _journalDetailRepository.Queryable().Where(d => lstDocIds.Contains(d.DocumentId)).ToList();
        }
        public Dictionary<Guid,decimal?> GetAllJDByIds(List<Guid> lstIds)
        {
            return _journalDetailRepository.Queryable().Where(d => lstIds.Contains(d.Id)).ToDictionary(Id=>Id.Id,Credit=>Credit.DocCredit);
        }
    }
}
