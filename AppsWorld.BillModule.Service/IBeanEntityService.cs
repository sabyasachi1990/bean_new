using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
    public interface IBeanEntityService : IService<BeanEntity>
    {
        BeanEntity GetEntityById(Guid id);
        BeanEntity GetByEntityId(Guid id, long companyId);
        string GetEntityNameById(Guid id);
        Guid GetExternalEntity(Guid exDocumentId);
        List<BeanEntity> GetListOfEntity(long companyId);
        string GetEntityName(long companyId, Guid entityId);
        //BeanEntity GetHRClaimsId(Guid id, long? companyId);




        BeanEntity GetEntityByName(string entitnyName, long? companyId);
    }
}
