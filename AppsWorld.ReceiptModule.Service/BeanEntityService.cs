using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEntityService
    {
        private readonly IReceiptModuleRepositoryAsync<BeanEntity> _beanEntityRepository;

		public BeanEntityService(IReceiptModuleRepositoryAsync<BeanEntity> beanEntityRepository)
			: base(beanEntityRepository)
        {
			_beanEntityRepository = beanEntityRepository;
        }
		public BeanEntity GetEntityById(Guid id)
		{
			return _beanEntityRepository.Query(c => c.Id == id).Select().FirstOrDefault();
		}
        public string GetEntityNameById(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(c=>c.Name).FirstOrDefault();
        }
        public decimal? GetEntityCreditTermsValue(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select(c => c.CreditLimitValue).FirstOrDefault();
        }
    }
}
