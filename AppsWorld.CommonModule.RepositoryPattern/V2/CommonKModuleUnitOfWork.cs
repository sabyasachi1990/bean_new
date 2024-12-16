using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.RepositoryPattern.V2
{
    public class CommonKModuleUnitOfWork : UnitOfWork, ICommonKModuleUnitOfWorkAsync
    {
        public CommonKModuleUnitOfWork(ICommonKModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
