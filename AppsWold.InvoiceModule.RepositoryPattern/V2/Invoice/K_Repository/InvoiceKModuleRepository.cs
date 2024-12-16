using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class InvoiceKModuleRepository<TEntity> : Repository<TEntity>, IInvoiceKModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public InvoiceKModuleRepository(IInvoiceKModuleDataContextAsync dataContext, IInvoiceKModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {
        }
    }
}
