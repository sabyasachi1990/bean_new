using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.RepositoryPattern.V2
{
    public class RevaluationUnitOfWork : UnitOfWork,IRevaluationUnitOfWorkAsync
    {
        public RevaluationUnitOfWork(IRevaluationDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
