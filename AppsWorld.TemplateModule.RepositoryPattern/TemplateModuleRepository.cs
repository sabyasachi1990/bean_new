
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.RepositoryPattern
{
   public class TemplateModuleRepository<TEntity> : Repository<TEntity>, ITemplateModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public TemplateModuleRepository(ITemplateModuleDataContextAsync dataContext, ITemplateModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    
    }
}
