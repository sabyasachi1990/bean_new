using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.BillModule.RepositoryPattern
{
    public class BillModuleRepository<TEntity> : Repository<TEntity>, IBillModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public BillModuleRepository(IBillModuleDataContextAsync dataContext, IBillModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
