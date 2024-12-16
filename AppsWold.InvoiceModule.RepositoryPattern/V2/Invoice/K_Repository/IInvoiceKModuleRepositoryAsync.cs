using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public interface IInvoiceKModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {

    }
}
