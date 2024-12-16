using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IBankReconciliationModuleRepositoryAsync<Journal> _JournalRepository;
        private readonly IBankReconciliationModuleRepositoryAsync<JournalDetail> _journalDetailRepository;

        public JournalService(IBankReconciliationModuleRepositoryAsync<Journal> JournalRepository, IBankReconciliationModuleRepositoryAsync<JournalDetail> journalDetailRepository)
            : base(JournalRepository)
        {
            this._JournalRepository = JournalRepository;
            this._journalDetailRepository = journalDetailRepository;
        }
        public List<Journal> GetAllJournal(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate, bool isClearing, bool isBankReconcile)
        {
            fromDate = fromDate.Value.Date;
            toDate = toDate.Date;
            if (isBankReconcile)
                return _JournalRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid && a.IsBankReconcile == true && (a.DocDate >= fromDate && a.DocDate <= toDate)).Select().ToList();
            if (isClearing)
                return _JournalRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.DocDate >= fromDate && a.DocDate <= toDate)).Select().ToList();
            else
                return _JournalRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.DocDate >= fromDate && a.DocDate <= toDate) && (a.ClearingDate == null)).Select().ToList();
        }

        //public List<Journal> GetAllJournalK(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate)
        //{
        //    if (isClearing)
        //        return _JournalRepository.Query(a => (a.DocType == "Deposit" || a.DocType == "Receipt" || a.DocType == "Payment" || a.DocType == "Transfer" || a.DocType == "Cash Sales") && (a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid) && (a.DocDate >= fromDate && a.DocDate <= toDate)).Select().ToList();
        //    else
        //        return _JournalRepository.Query(a => (a.DocType == "Deposit" || a.DocType == "Receipt" || a.DocType == "Payment" || a.DocType == "Transfer" || a.DocType == "Cash Sales") && (a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid) && (a.DocDate >= fromDate && a.DocDate <= toDate) && a.ClearingDate == null).Select().ToList();
        //}
        public Journal GetJournal(Guid id, long companyId)
        {
            return _JournalRepository.Query(a => a.DocumentId == id && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<JournalDetail> GetAllJournalDetails(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate, bool isClearing, bool isBankReconcile, DateTime? lastReconDate)
        {
            //DateTime? lastReconDate = _journalDetailRepository.Queryable().Where(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && a.ReconciliationId != null && a.ReconciliationDate != null).OrderByDescending(c => c.ReconciliationDate).Select(c => c.ReconciliationDate).FirstOrDefault();
            //List<JournalDetail> lstJD = new List<JournalDetail>();
            List<JournalDetail> newLstJD = new List<JournalDetail>();
            if (fromDate != null && toDate != null)
            {
                fromDate = fromDate.Value.Date;
                toDate = toDate != null ? toDate.Value.Date : (DateTime?)null;

                //return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && ((a.DocDate >= fromDate && a.DocDate <= toDate) || a.ReconciliationDate >= lastReconDate) && (a.IsTax == false || a.IsTax == null)).Select().ToList();

                newLstJD = _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.IsTax == false || a.IsTax == null) && (a.DocDate >= fromDate && a.DocDate <= toDate)).Select().ToList();
                //if (newLstJD.Any())
                //{
                //    List<JournalDetail> newLstJD1 = newLstJD.Where(a => a.DocDate >= fromDate && a.DocDate <= toDate && a.ReconciliationDate == null && a.ReconciliationId == null && a.ClearingDate == null).ToList();
                //    if (newLstJD1.Any())
                //        lstJD.AddRange(newLstJD1);
                //    newLstJD = newLstJD.Where(c => /*c.ReconciliationDate != null && (*/c.ReconciliationDate >= lastReconDate && c.ClearingDate >= lastReconDate).ToList();
                //    lstJD.AddRange(newLstJD);
                //}
            }
            //else if (fromDate == null && toDate == null)
            //{
            //    //if (isBankReconcile)
            //    //    return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && a.IsBankReconcile == true && (a.IsTax == false || a.IsTax == null)).Select().ToList();
            //    //if (isClearing)
            //    //    return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.IsTax == false || a.IsTax == null)).Select().ToList();
            //    //else
            //    return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.ClearingDate == null) && (a.IsTax == false || a.IsTax == null) && a.DocumentDetailId != new Guid()).Select().ToList();
            //}
            else
            {
                toDate = toDate != null ? toDate.Value.Date.AddDays(1) : (DateTime?)null;
                toDate = toDate != null ? toDate.Value.AddSeconds(-1) : (DateTime?)null;

                //return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && a.DocDate <= toDate && a.ReconciliationDate >= lastReconDate && (a.IsTax == false || a.IsTax == null)).Select().ToList();
                newLstJD = _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.IsTax == false || a.IsTax == null) && a.DocDate <= toDate).Select().ToList();
                //if (newLstJD.Any())
                //{
                //    List<JournalDetail> newLstJD1 = newLstJD.Where(a => a.DocDate <= toDate && a.ReconciliationDate == null && a.ReconciliationId == null && a.ClearingDate == null).ToList();
                //    if (newLstJD1.Any())
                //        lstJD.AddRange(newLstJD1);
                //    newLstJD = newLstJD.Where(c => /*c.ReconciliationDate != null && (*/c.ReconciliationDate >= lastReconDate && c.ClearingDate >= lastReconDate).ToList();
                //    lstJD.AddRange(newLstJD);
                //}
            }
            return newLstJD;
        }

        public List<JournalDetail> GetAllCleraingJournalDetails(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate, bool isClearing)
        {
            fromDate = fromDate.Value.Date;
            toDate = toDate.Date;
            return _journalDetailRepository.Query(a => a.ServiceCompanyId == subcompanyId && a.COAId == chartid && (a.DocDate >= fromDate && a.DocDate <= toDate) && (a.ClearingDate != null) && a.IsTax == false).Select().ToList();
        }
        public List<JournalDetail> GetlstJournalDetailByCoaId(long serviceCompanyId, long coaId, DateTime? reconciledDate, DateTime? lastReconciledDate)
        {
            if (lastReconciledDate != null)
            {
                return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && (a.DocDate <= reconciledDate /*&&*/|| a.DocDate > lastReconciledDate) && (a.ClearingDate == null || (a.ClearingDate > reconciledDate || a.ClearingDate > lastReconciledDate))).Select().ToList();

                //old one modified on 01/06/2019
                //return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && (a.DocDate <= reconciledDate || a.DocDate > lastReconciledDate) && (a.ClearingDate == null || (a.ClearingDate > reconciledDate && a.ClearingDate > lastReconciledDate))).Select().ToList();



                //var journalList = ;
                //long companyId = 1;
                //return (from j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId)
                //        join jd in _journalDetailRepository.Queryable() on j.Id equals jd.JournalId
                //        where j.ServiceCompanyId == serviceCompanyId && j.COAId == coaId && (jd.DocDate <= reconciledDate && jd.DocDate > lastReconciledDate) && (jd.ClearingDate == null || jd.ClearingDate > lastReconciledDate)).ToList();

            }
            else
                return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.DocDate <= reconciledDate).Select().ToList();
        }
        public List<JournalDetail> GetlstJournalDetailBySubIdandCoaId(long serviceCompanyId, long coaId, DateTime? reconciledDate, DateTime? lastReconciledDate, List<Guid> lstGuid)
        {
            if (lastReconciledDate != null)
            {
                return _journalDetailRepository.Query(a => !lstGuid.Contains(a.Id) && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && (a.DocDate <= reconciledDate && a.DocDate > lastReconciledDate) && (a.ClearingDate == null || a.ClearingDate > lastReconciledDate)).Select().ToList();

                //var journalList = ;
                //long companyId = 1;
                //return (from j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId)
                //        join jd in _journalDetailRepository.Queryable() on j.Id equals jd.JournalId
                //        where j.ServiceCompanyId == serviceCompanyId && j.COAId == coaId && (jd.DocDate <= reconciledDate && jd.DocDate > lastReconciledDate) && (jd.ClearingDate == null || jd.ClearingDate > lastReconciledDate)).ToList();

            }
            else
                return _journalDetailRepository.Query(a => !lstGuid.Contains(a.Id) && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.DocDate <= reconciledDate).Select().ToList();
        }
        public List<Guid?> GetListJournalDetailIds(long serviceCompanyId, long coaId, DateTime? reconciledDate, DateTime? lastReconciledDate)
        {
            if (lastReconciledDate != null)
            {
                return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && (a.DocDate <= reconciledDate && a.DocDate > lastReconciledDate) && (a.ClearingDate == null || a.ClearingDate > lastReconciledDate)).Select(a => a.DocumentId).ToList();

                //var journalList = ;
                //long companyId = 1;
                //return (from j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId)
                //        join jd in _journalDetailRepository.Queryable() on j.Id equals jd.JournalId
                //        where j.ServiceCompanyId == serviceCompanyId && j.COAId == coaId && (jd.DocDate <= reconciledDate && jd.DocDate > lastReconciledDate) && (jd.ClearingDate == null || jd.ClearingDate > lastReconciledDate)).ToList();

            }
            else
                return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.DocDate <= reconciledDate).Select(a => a.DocumentId).ToList();
        }
        public async Task<decimal?> GetGLBalance(long coaId, long companyId, long serviceCompanyId, DateTime reconciledDate)
        {
            //DateTime? systemStartDate = _JournalRepository.Query(a => a.COAId == coaId && a.ServiceCompanyId == serviceCompanyId).Select().OrderByDescending(a => a.CreatedDate).Select(a => a.PostingDate).FirstOrDefault();
            //DateTime? systemStartDate = _JournalRepository.Query(a => a.CompanyId == companyId).Select().OrderBy(a => a.CreatedDate).Select(a => a.PostingDate).FirstOrDefault();

            //var systemStartDate = (from i in _JournalRepository.Queryable() where i.CompanyId == companyId orderby i.PostingDate ascending select i.PostingDate).FirstOrDefault();
            //List<JournalDetail> lstJDetail = _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && (a.DocDate >= systemStartDate && a.DocDate <= reconciledDate)).Select().ToList();
            //decimal? glBalance = Math.Round((decimal)lstJDetail.Sum(a => a.BaseDebit) - (decimal)lstJDetail.Sum(a => a.BaseCredit), 2, MidpointRounding.AwayFromZero);

            var systemStartDate = (from i in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId) orderby i.PostingDate ascending select i.PostingDate).FirstOrDefault();

            List<JournalDetail> lstJDetail = await Task.FromResult((from jd in _journalDetailRepository.Queryable().Where(a => a.DocDate >= systemStartDate && a.DocDate <= reconciledDate)
                                              join j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
                                              where j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked"
                                              && jd.ServiceCompanyId==serviceCompanyId&&jd.COAId==coaId
                                              select jd).ToList());

            decimal? glBalance = Math.Round((decimal)lstJDetail.Sum(a => a.DocDebit) - (decimal)lstJDetail.Sum(a => a.DocCredit), 2, MidpointRounding.AwayFromZero);


            return glBalance;
        }
        public List<JournalDetail> GetListOfJournalDetail(long coaId, long serviceCompanyId)
        {
            return _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId /*&& (a.DocDate >= systemStartDate && a.DocDate <= reconciledDate)*/).Select().ToList();

            //var systemStartDate = (from i in _JournalRepository.Queryable() where i.CompanyId == companyId orderby i.PostingDate ascending select i.PostingDate).FirstOrDefault();

            //List<JournalDetail> lstJDetail = (from jd in _journalDetailRepository.Queryable()
            //                                  join j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
            //                                  where (j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked" && (jd.DocDate >= systemStartDate && jd.DocDate <= reconciledDate))
            //                                  select jd).ToList()

            //decimal? glBalance = Math.Round((decimal)lstJDetail.Sum(a => a.BaseDebit) - (decimal)lstJDetail.Sum(a => a.BaseCredit), 2, MidpointRounding.AwayFromZero);

        }
        public List<JournalDetail> GetListOfClearedDetail(long companyId, long serviceCompanyId, long coaId, DateTime? brcReocnDate)
        {
            return (from jd in _journalDetailRepository.Queryable()
                    join j in _JournalRepository.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
                    where (j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked" && jd.ClearingStatus == "Cleared" && jd.ServiceCompanyId == serviceCompanyId && jd.COAId == coaId && jd.ClearingDate <= brcReocnDate && jd.DocDate <= brcReocnDate)
                    select jd).ToList();
        }
        public List<JournalDetail> lstJournalDetail(Guid? reconciliationId, long coaId, long serviceCompanyId)
        {
            return _journalDetailRepository.Query(a => a.ReconciliationId == reconciliationId && a.COAId == coaId && a.ServiceCompanyId == serviceCompanyId).Select().ToList();
        }
        public List<Journal> GetListOfJournalByJournalId(long companyId, List<Guid> lstJournalIds)
        {
            return _JournalRepository.Query(a => a.CompanyId == companyId && lstJournalIds.Contains(a.Id) && (a.DocumentState != "Void" && a.DocumentState != "Reset" && a.DocumentState != "Recurring" && a.DocumentState != "Parked")).Select().ToList();
        }

        public bool IfAnyJournalVoid(long companyId, List<Guid?> lstDocumentIds)
        {
            return _JournalRepository.Query(a => a.CompanyId == companyId && lstDocumentIds.Contains(a.DocumentId) && a.DocumentState == "Void").Select(a => a.DocumentState).Any();
        }

    }
}
