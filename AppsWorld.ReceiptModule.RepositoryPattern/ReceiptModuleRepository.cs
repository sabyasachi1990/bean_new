using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.ReceiptModule.RepositoryPattern
{
    public class ReceiptModuleRepository<TEntity> : Repository<TEntity>, IReceiptModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ReceiptModuleRepository(IReceiptModuleDataContextAsync dataContext, IReceiptModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
