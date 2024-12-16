using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.RepositoryPattern
{
    public class RevaluationModuleUnitOfWork:UnitOfWork,IRevaluationModuleUnitOfWorkAsync
    {
        public RevaluationModuleUnitOfWork(IRevaluationModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
