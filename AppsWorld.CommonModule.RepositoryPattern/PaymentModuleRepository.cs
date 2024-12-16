using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.CommonModule.RepositoryPattern
{
    public class CommonModuleRepository<TEntity> : Repository<TEntity>, ICommonModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public CommonModuleRepository(ICommonModuleDataContextAsync dataContext, ICommonModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
