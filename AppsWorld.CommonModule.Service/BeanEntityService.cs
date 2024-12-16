using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEntityService
    {
        private readonly ICommonModuleRepositoryAsync<BeanEntity> _beanEntityRepository;

        public BeanEntityService(ICommonModuleRepositoryAsync<BeanEntity> beanEntityRepository)
            : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
        }
        public BeanEntity GetEntityById(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public async Task<List<BeanEntity>> GetAllEntities(string type, long companyId, Guid? entityId)
        {
            if (type == "Customer")
                return await Task.Run(()=> _beanEntityRepository.Query(c => (c.Status == RecordStatusEnum.Active || c.Id == entityId) && c.IsCustomer == true && c.CompanyId == companyId /*&& c.IsShowPayroll == true*/ && c.CustNature != "Interco").Select().ToList());
            else
                return await Task.Run(()=> _beanEntityRepository.Query(c => c.IsVendor == true && (c.Status == RecordStatusEnum.Active || c.Id == entityId) && c.CompanyId == companyId /*&& c.IsShowPayroll == true*/&& c.VenNature != "Interco").Select().ToList());
        }
        public List<BeanEntity> GetEntityByCId(long Companyid)
        {
            return _beanEntityRepository.Query(c => c.CompanyId == Companyid).Select().ToList();
        }

        public List<BeanEntity> GetEntityByCompanyId(long companyId)
        {
            return _beanEntityRepository.Query(b => b.CompanyId == companyId && b.IsShowPayroll == false).Select().ToList();
        }
        public decimal? GetCteditLimitsValue(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(d => d.CreditLimitValue).FirstOrDefault();
        }
        public string GetEntityName(Guid? id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(d => d.Name).FirstOrDefault();
        }
        public Dictionary<Guid, string> GetListOfEntity(long companyId, List<Guid?> entityId)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && entityId.Contains(a.Id)).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        }
        public async Task<List<BeanEntity>> GetListOfEntity(long companyId, Guid? entityId)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.CompanyId == companyId && (a.Status == RecordStatusEnum.Active || a.Id == entityId)).Select().ToList());
        }
        public string GetEntityName(long companyId, Guid entityId)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && a.Id == entityId).Select(c => c.Name).FirstOrDefault();
        }
        public Dictionary<long?, Guid> GetListOfEntityIdsandServiceEntityId(long companyId, List<long?> serviceEntityIds)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && serviceEntityIds.Contains(a.ServiceEntityId)).Select(a => new { a.ServiceEntityId, a.Id }).ToDictionary(t => t.ServiceEntityId, t => t.Id);
        }
    }
}
