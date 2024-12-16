using Repository.Pattern.Ef6;

namespace AppsWorld.ReceiptModule.RepositoryPattern.V2
{
    public class ReceiptKModuleUnitOfWork : UnitOfWork, IReceiptKModuleUnitOfWorkAsync
    {
        public ReceiptKModuleUnitOfWork(IReceiptKModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
