using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.CommonModule.RepositoryPattern.V2
{
    public interface ICommonKModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState { }
}
