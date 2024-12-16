using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.RepositoryPattern
{
    public class MasterModuleRepository<TEntity> : Repository<TEntity>, IMasterModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
       public MasterModuleRepository(IMasterModuleDataContextAsync dataContext, IMasterModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
       {
       }
    }
}
