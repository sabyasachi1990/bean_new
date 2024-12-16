using Service.Pattern;
using System;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using System.Collections.Generic;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEntityService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<BeanEntity> _beanEntityRepository;

        public BeanEntityService(IJournalVoucherModuleRepositoryAsync<BeanEntity> beanEntityRepository)
            : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
        }
        public BeanEntity GetEntityById(Guid? id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public Dictionary<Guid, string> GetEntityByid(List<Guid?> entityIds, long companyId)
        {
            return _beanEntityRepository.Query(a => entityIds.Contains(a.Id) && a.CompanyId == companyId).Select(c => new { c.Id, c.Name }).ToDictionary(c => c.Id, c => c.Name);
        }
        public string GetEntityByid(Guid? entityIds, long companyId)
        {
            return _beanEntityRepository.Query(a => a.Id == entityIds && a.CompanyId == companyId).Select(a => a.Name).FirstOrDefault();
        }
        public decimal? GetCteditLimitsValue(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(d => d.CustCreditLimit).FirstOrDefault();
        }

    }
}
