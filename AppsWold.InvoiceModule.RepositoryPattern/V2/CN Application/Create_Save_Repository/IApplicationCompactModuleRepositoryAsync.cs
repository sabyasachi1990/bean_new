using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public interface IApplicationCompactModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {

    }
}
