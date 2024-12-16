using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public interface IRevaluationService : IService<Revaluation>
    {
        IQueryable<RevaluationModelK> GetAllRevaluationsK(string username, long companyId);
        Revaluation GetAllRevaluationById(Guid id, long companyId);
        List<Revaluation> GetAllRevaluationCompanyId(long companyId);
        List<Revaluation> GetAllPostedRevaluation(DateTime? revDate, long? serviceCompanyId);
        Revaluation GetAllRevaluationAndDetail(Guid id, long companyId);
    }
}
