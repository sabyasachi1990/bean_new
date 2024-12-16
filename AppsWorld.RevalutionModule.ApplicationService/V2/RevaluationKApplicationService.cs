using System.Linq;
using AppsWorld.RevaluationModule.Models;
using AppsWorld.RevaluationModule.Service.V2;

namespace AppsWorld.RevaluationModule.Application.V2
{
    public class RevaluationKApplicationService
    {
        readonly IRevaluationKService _revaluationService;
        public RevaluationKApplicationService(IRevaluationKService revaluationService)
        {
            this._revaluationService = revaluationService;
        }

        #region Grid_Call
        public IQueryable<RevaluationModelK> GetAllRevaluationK(string username, long companyId)
        {
            return _revaluationService.GetAllRevaluationsK(username, companyId);
        }
        #endregion

    }
}
