using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.CashSalesModule.RepositoryPattern
{
    public class CashSalesRepository<TEntity> : Repository<TEntity>, ICashSalesModuleRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public CashSalesRepository(ICashSalesModuleDataContextAsync dataContext, ICashSalesModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
