using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class InvoiceComptModuleRepository<TEntity> : Repository<TEntity>, IInvoiceComptModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public InvoiceComptModuleRepository(IInvoiceComptModuleDataContextAsync dataContext, IInvoiceComptModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {
        }
    }
}
