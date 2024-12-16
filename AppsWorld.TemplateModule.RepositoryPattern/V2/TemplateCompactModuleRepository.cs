
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.RepositoryPattern.V2
{
   public class TemplateCompactModuleRepository<TEntity> : Repository<TEntity>, ITemplateCompactModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public TemplateCompactModuleRepository(ITemplateCompactModuleDataContextAsync dataContext, ITemplateCompactModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    
    }
}
