using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class RevaluationDetailService : Service<RevalutionDetail>, IRevaluationDetailService
    {
        private readonly IRevaluationModuleRepositoryAsync<RevalutionDetail> _revaluationDetailRepository;
        public RevaluationDetailService(IRevaluationModuleRepositoryAsync<RevalutionDetail> revaluationDetailRepository)
            : base(revaluationDetailRepository)
        {
            _revaluationDetailRepository = revaluationDetailRepository;
        }
        public List<RevalutionDetail> GetDetails(Guid revaluationId)
        {
            return _revaluationDetailRepository.Query(c => c.RevalutionId == revaluationId).Select().ToList();
        }
    }
}
