using System;
using System.Collections.Generic;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.V2;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public interface IRevaluationService : IService<Revaluation>
    {
        Revaluation GetRevaluationById(Guid id, long companyId);
        List<Revaluation> GetAllRevaluationCompanyId(long companyId);
        bool GetAllPostedRevaluation(DateTime? revDate, long? serviceCompanyId);
        Revaluation GetAllRevaluationAndDetail(Guid id, long companyId);
        List<RevalutionDetail> GetDetails(Guid revaluationId);
        List<string> GetJDDocCurrecies(List<long> serviceCompanyId);
        Revaluation GetRevalForVoid(Guid id, long companyId);
        void RevalDetailInsert(RevalutionDetail detail);
        List<long?> GetAllRevaluedCOAIds(DateTime? revDate, List<long> serviceCompanyId);
        List<JournalCompact> GetAllJournalByCompanyId(long companyId);
    }
}
