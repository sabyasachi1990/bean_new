using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.ReceiptModule.RepositoryPattern
{
    public interface IReceiptModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState { }
}
