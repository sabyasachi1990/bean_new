using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.RepositoryPattern
{
    public class CommonModuleUnitOfWork : UnitOfWork, ICommonModuleUnitOfWorkAsync
    {
        public CommonModuleUnitOfWork(ICommonModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
