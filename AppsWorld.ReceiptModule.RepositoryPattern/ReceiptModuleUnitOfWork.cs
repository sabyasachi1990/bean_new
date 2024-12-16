using Repository.Pattern.Ef6;

namespace AppsWorld.ReceiptModule.RepositoryPattern
{
    public class ReceiptModuleUnitOfWork : UnitOfWork, IReceiptModuleUnitOfWorkAsync
    {
        public ReceiptModuleUnitOfWork(IReceiptModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
