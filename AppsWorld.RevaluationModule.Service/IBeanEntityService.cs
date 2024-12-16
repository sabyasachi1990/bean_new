using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public interface  IBeanEntityService
    {
        BeanEntity GetEntityById(Guid id);
        List<BeanEntity> GetAllEntityById(List<Guid> ids);
    }
}
