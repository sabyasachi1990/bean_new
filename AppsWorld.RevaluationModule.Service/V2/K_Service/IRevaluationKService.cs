using System.Linq;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.V2;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public interface IRevaluationKService : IService<RevaluationK>
    {
        IQueryable<RevaluationModelK> GetAllRevaluationsK(string username, long companyId);
    }
}
