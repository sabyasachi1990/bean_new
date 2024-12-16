using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.ReceiptModule.RepositoryPattern.V2
{
    public interface IReceiptKModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState { }
}
