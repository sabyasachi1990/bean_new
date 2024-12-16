using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.RepositoryPattern.V2
{
    public class RevaluationKUnitOfWork : UnitOfWork,IRevaluationKUnitOfWorkAsync
    {
        public RevaluationKUnitOfWork(IRevaluationKDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
