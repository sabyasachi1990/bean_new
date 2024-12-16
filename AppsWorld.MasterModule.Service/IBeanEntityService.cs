using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Entities.Models;
using AppsWorld.MasterModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IBeanEntityService : IService<BeanEntity>
    {
        IQueryable<BeanEntityModelk> GetAllBeanEntitysK(long companyId);
        IEnumerable<BeanEntity> GetAllBeanEntities(long companyId);
        Task<BeanEntity> GetBeanEntities(long CompanyId, Guid id);
        BeanEntity GetBeanEntityByIdAndCompanyId(long CompanyId, Guid Id);
        BeanEntity GetBeanEntityNameCheck(Guid Id, string Name, long CompanyId);
        List<BeanEntity> GetBeanEntityNameChec(string Name, long CompanyId);
        Task<List<BeanEntity>> GetAllBeanEntitys(long CompanyId);
        IQueryable<BeanEntity> GetAllBeanEntitysNew(long CompanyId);
        Task<List<BeanEntity>> GetAllBeanEntity(Guid? Id, long CompanyId);
        Task<List<BeanEntity>> GetBeanEntityByVendor(long CompanyId);
        List<BeanEntity> GetByEntityId(Guid? EntityId, long CompanyId);
        BeanEntity GetBeanEntityByDocumentId(Guid documentId);
        BeanEntity GetBeanEntityByDocumentIdCid(Guid documentId, long companyId, string name);
        BeanEntity GetBeanEntityByClientId(long CompanyId, Guid Id);
        //List<BeanEntity> GetAllVendorEntities(long CompanyId, Guid? id);
        bool? IfBeanEntityExists(Guid entityId, long companyId, string name);
        List<BeanEntity> GetListOfEntity(long companyId);
        BeanEntity GetBeanEntity(long CompanyId, long coaId);
        Task<List<BeanEntity>> GetAllBeanEntitiesExpInv(long CompanyId);
        IQueryable<BeanEntity> GetAllBeanEntitiesExpInvNew(long CompanyId);
        Task<List<BeanEntity>> GetAllBeanEntityExpInv(Guid? Id, long CompanyId);
        Task<List<BeanEntity>> GetBeanEntityByVendorExpBill(long CompanyId);
        IQueryable<LinkedAccountsModel> GetLinkedAccountsK(long? companyId, string connectionString,string username);

        //for Entity name is exist or not.
        bool IfEntityNameExistsByNature(long companyId, Guid entityId, string name, string nature);
        List<SSICCodes> GetSSICCodesByType(string type);
        Task<BeanEntity> GetBeanEntitiesAsync(long CompanyId, Guid id);
       
    }
}
