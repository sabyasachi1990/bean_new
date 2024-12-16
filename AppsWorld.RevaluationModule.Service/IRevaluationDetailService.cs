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
    public interface IRevaluationDetailService : IService<RevalutionDetail>
    {

        List<RevalutionDetail> GetDetails(Guid revaluationId);
    }
}
