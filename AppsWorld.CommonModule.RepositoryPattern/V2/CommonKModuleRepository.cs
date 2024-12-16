using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.CommonModule.RepositoryPattern.V2
{
    public class CommonKModuleRepository<TEntity> : Repository<TEntity>, ICommonKModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public CommonKModuleRepository(ICommonKModuleDataContextAsync dataContext, ICommonKModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
