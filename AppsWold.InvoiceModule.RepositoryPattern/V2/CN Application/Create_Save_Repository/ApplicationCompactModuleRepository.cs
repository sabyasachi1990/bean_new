using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class ApplicationCompactModuleRepository<TEntity> : Repository<TEntity>, IApplicationCompactModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ApplicationCompactModuleRepository(IApplicationCompactModuleDataContextAsync dataContext, IApplicationCompactModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {
        }
    }
}
