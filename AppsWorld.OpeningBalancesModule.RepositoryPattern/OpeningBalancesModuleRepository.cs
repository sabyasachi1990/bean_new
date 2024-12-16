using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;

namespace AppsWorld.OpeningBalancesModule.RepositoryPattern
{
   public class OpeningBalancesModuleRepository<TEntity> : Repository<TEntity>, IOpeningBalancesModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
       public OpeningBalancesModuleRepository(IOpeningBalancesModuleDataContextAsync dataContext, IOpeningBalancesModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
       {
       }
    }
}
