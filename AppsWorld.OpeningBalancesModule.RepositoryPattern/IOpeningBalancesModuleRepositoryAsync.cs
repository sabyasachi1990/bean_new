using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.OpeningBalancesModule.RepositoryPattern
{
   public interface IOpeningBalancesModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
    }
}
