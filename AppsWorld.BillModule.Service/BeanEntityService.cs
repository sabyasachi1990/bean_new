using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.BillModule.Service;

namespace AppsWorld.BillModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEntityService
    {
        private readonly IBillModuleRepositoryAsync<BeanEntity> _beanEntityRepository;

        public BeanEntityService(IBillModuleRepositoryAsync<BeanEntity> beanEntityRepository)
            : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
        }
        public BeanEntity GetEntityById(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }

        public BeanEntity GetByEntityId(Guid id, long companyId)
        {
            return _beanEntityRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public string GetEntityNameById(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(x => x.Name).FirstOrDefault();
        }
        public Guid GetExternalEntity(Guid exDocumentId)
        {
            return _beanEntityRepository.Query(c => c.DocumentId == exDocumentId).Select(x => x.Id).FirstOrDefault();
        }

        public List<BeanEntity> GetListOfEntity(long companyId)
        {
            List<BeanEntity> lstBeanEntity = _beanEntityRepository.Query(v => v.CompanyId == companyId).Select().ToList();
            return lstBeanEntity;
        }
        public string GetEntityName(long companyId, Guid entityId)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && a.Id == entityId).Select(c => c.Name).FirstOrDefault();
        }
        //public BeanEntity GetHRClaimsId(Guid id, long? companyId)
        //{
        //    return _beanEntityRepository.Query(c => c.DocumentId == id && c.CompanyId == companyId).Select().FirstOrDefault();
        //}



        public BeanEntity GetEntityByName(string entitnyName, long? companyId)
        {
           return _beanEntityRepository.Query(x => x.Name.ToLower() == entitnyName.ToLower() && x.CompanyId == companyId).Select().FirstOrDefault();
        }

       
    }
}
