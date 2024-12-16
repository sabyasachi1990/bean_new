using Repository.Pattern.Ef6;

namespace AppsWorld.BillModule.RepositoryPattern
{
    public class BillModuleUnitOfWork : UnitOfWork, IBillModuleUnitOfWorkAsync
    {
        public BillModuleUnitOfWork(IBillModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
