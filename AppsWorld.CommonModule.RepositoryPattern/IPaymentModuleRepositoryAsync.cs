using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.CommonModule.RepositoryPattern
{
    public interface ICommonModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState { }
}
