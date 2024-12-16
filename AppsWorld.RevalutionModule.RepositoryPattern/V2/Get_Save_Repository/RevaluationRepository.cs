using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.RevaluationModule.RepositoryPattern.V2
{
    public class RevaluationRepository<TEntity>: Repository<TEntity>, IRevaluationRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public RevaluationRepository(IRevaluationDataContextAsync dataContext, IRevaluationUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
