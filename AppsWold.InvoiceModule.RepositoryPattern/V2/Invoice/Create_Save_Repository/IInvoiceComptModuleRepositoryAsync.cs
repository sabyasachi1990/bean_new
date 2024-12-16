using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public interface IInvoiceComptModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {

    }
}
