using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.V2;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;
using AppsWorld.RevaluationModule.Infra;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public class RevaluationService : Service<Revaluation>, IRevaluationService
    {
        readonly IRevaluationRepositoryAsync<Revaluation> _revaluationRepository;
        readonly IRevaluationRepositoryAsync<RevalutionDetail> _revaluationDetailRepository;
        readonly IRevaluationRepositoryAsync<JournalDetailCompact> _journalDetailRepository;
        readonly IRevaluationRepositoryAsync<JournalCompact> _journalRepository;
        public RevaluationService(IRevaluationRepositoryAsync<Revaluation> revaluationRepository, IRevaluationRepositoryAsync<RevalutionDetail> revaluationDetailRepository, IRevaluationRepositoryAsync<JournalDetailCompact> journalDetailRepository, IRevaluationRepositoryAsync<JournalCompact> journalRepository)
            : base(revaluationRepository)
        {
            _revaluationRepository = revaluationRepository;
            _revaluationDetailRepository = revaluationDetailRepository;
            _journalDetailRepository = journalDetailRepository;
            _journalRepository = journalRepository;
        }
        public Revaluation GetRevaluationById(Guid id, long companyId)
        {
            return _revaluationRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Revaluation GetRevalForVoid(Guid id, long companyId)
        {
            return _revaluationRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.DocState != RevaluationConstant.Void).Select().FirstOrDefault();
        }
        public List<Revaluation> GetAllRevaluationCompanyId(long companyId)
        {
            return _revaluationRepository.Queryable().Where(x => x.CompanyId == companyId).ToList();
        }
        public bool GetAllPostedRevaluation(DateTime? revDate, long? serviceCompanyId)
        {
            return _revaluationRepository.Queryable().Where(x => x.RevalutionDate == revDate && x.ServiceCompanyId == serviceCompanyId && x.DocState == RevaluationConstant.Posted).Any();
        }
        public Revaluation GetAllRevaluationAndDetail(Guid id, long companyId)
        {
            return _revaluationRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(d => d.RevalutionDetails).Select().FirstOrDefault();
        }
        public List<RevalutionDetail> GetDetails(Guid revaluationId)
        {
            return _revaluationDetailRepository.Query(c => c.RevalutionId == revaluationId).Select().ToList();
        }
        public List<string> GetJDDocCurrecies(List<long> serviceCompanyId)
        {
            //var d = _journalDetailRepository.Queryable().Where(c => serviceCompanyId.Contains(c.ServiceCompanyId) && c.DocCurrency != c.BaseCurrency).GroupBy(c => new { Base = c.BaseCurrency, Doc = c.DocCurrency }).ToList();
            //return _journalDetailRepository.Queryable().Where(c => serviceCompanyId.Contains(c.ServiceCompanyId) && c.DocCurrency != null && c.BaseCurrency != null).GroupBy(c => new { Base = c.BaseCurrency, Doc = c.DocCurrency }).Select(c => new { BaseCurr = c.Key.Base, DocCurr = c.Key.Doc }).ToDictionary(bas => bas.BaseCurr, doc => doc.DocCurr);
            return _journalDetailRepository.Queryable().Where(c => serviceCompanyId.Contains(c.ServiceCompanyId.Value) && c.DocCurrency != null && c.DocCurrency != c.BaseCurrency).GroupBy(c => c.DocCurrency).Select(d => d.Key).ToList();
        }
        public List<long?> GetAllRevaluedCOAIds(DateTime? revDate, List<long> serviceCompanyId)
        {
            List<long?> lstCOAIds = null;
            List<RevalutionDetail> revalDetail = _revaluationRepository.Query(x => x.RevalutionDate == revDate && serviceCompanyId.Contains(x.ServiceCompanyId.Value) && x.DocState == RevaluationConstant.Posted).Include(c => c.RevalutionDetails).Select(c => c.RevalutionDetails).FirstOrDefault();
            if (revalDetail != null && revalDetail.Any())
                lstCOAIds = revalDetail.Where(c => serviceCompanyId.Contains(c.ServiceEntityId.Value) && c.IsChecked == true).Select(c => c.COAId).ToList();
            return lstCOAIds;
        }

        public void RevalDetailInsert(RevalutionDetail detail)
        {
            _revaluationDetailRepository.Insert(detail);
        }
        public List<JournalCompact> GetAllJournalByCompanyId(long companyId)
        {
            return _journalRepository.Queryable().Where(x => x.CompanyId == companyId).ToList();
        }
    }
}
