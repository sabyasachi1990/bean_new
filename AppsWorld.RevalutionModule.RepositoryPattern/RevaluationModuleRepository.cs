using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.RevaluationModule.RepositoryPattern
{
    public class RevaluationModuleRepository<TEntity>: Repository<TEntity>, IRevaluationModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public RevaluationModuleRepository(IRevaluationModuleDataContextAsync dataContext, IRevaluationModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
    
}
