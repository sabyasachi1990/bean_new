using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.RevaluationModule.RepositoryPattern.V2
{
    public class RevaluationKRepository<TEntity>: Repository<TEntity>, IRevaluationKRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public RevaluationKRepository(IRevaluationKDataContextAsync dataContext, IRevaluationKUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
