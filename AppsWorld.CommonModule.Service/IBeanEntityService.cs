using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
    public interface IBeanEntityService : IService<BeanEntity>
    {
        BeanEntity GetEntityById(Guid id);
        Task<List<BeanEntity>> GetAllEntities(string type, long companyId, Guid? entityId);
        List<BeanEntity> GetEntityByCId(long Companyid);
        List<BeanEntity> GetEntityByCompanyId(long companyId);
        decimal? GetCteditLimitsValue(Guid id);
        string GetEntityName(Guid? id);

        Dictionary<Guid, string> GetListOfEntity(long companyId, List<Guid?> entityId);//added by lokanath for BR list of Entity

        Task<List<BeanEntity>> GetListOfEntity(long companyId, Guid? entityId);
        string GetEntityName(long companyId, Guid entityId);
        Dictionary<long?, Guid> GetListOfEntityIdsandServiceEntityId(long companyId, List<long?> serviceEntityIds);
    
    }
}
