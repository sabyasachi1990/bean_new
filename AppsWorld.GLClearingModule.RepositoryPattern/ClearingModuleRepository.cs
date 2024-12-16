using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.RepositoryPattern
{
    public class ClearingModuleRepository<TEntity> : Repository<TEntity>, IClearingModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ClearingModuleRepository(IClearingModuleDataContextAsync dataContext, IClearingModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
       {
       }
    }
}
