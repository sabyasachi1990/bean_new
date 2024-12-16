using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class BeanEntityService:Service<BeanEntity>,IBeanEntityService
    {
        private readonly IRevaluationModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        public BeanEntityService(IRevaluationModuleRepositoryAsync<BeanEntity> beanEntityRepository)
            : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
        }
        public BeanEntity GetEntityById(Guid id)
        {
            return _beanEntityRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public List<BeanEntity> GetAllEntityById(List<Guid> ids)
        {
            return _beanEntityRepository.Query(c => ids.Contains(c.Id)).Select().ToList();
        }
    }
}
