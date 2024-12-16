using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;


namespace AppsWorld.BankWithdrawalModule.RepositoryPattern
{
    public class BankWithdrawalRepository<TEntity>: Repository<TEntity>, IBankWithdrawalModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public BankWithdrawalRepository(IBankWithdrawalModuleDataContextAsync dataContext, IBankWithdrawalModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
