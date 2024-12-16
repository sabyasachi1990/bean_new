using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public class CashSalesKRepository<TEntity> : Repository<TEntity>, ICashSalesKModuleRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public CashSalesKRepository(ICashSalesKModuleDataContextAsync dataContext, ICashSalesKModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
