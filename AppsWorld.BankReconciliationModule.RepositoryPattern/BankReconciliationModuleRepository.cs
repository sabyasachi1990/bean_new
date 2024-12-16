using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.BankReconciliationModule.RepositoryPattern
{
    public class BankReconciliationModuleRepository<TEntity> : Repository<TEntity>, IBankReconciliationModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public BankReconciliationModuleRepository(IBankReconciliationModuleDataContextAsync dataContext, IBankReconciliationModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
