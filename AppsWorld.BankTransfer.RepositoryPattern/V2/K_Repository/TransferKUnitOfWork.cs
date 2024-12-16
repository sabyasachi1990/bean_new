using Repository.Pattern.Ef6;

namespace AppsWorld.BankTransferModule.RepositoryPattern.V2
{
    public class TransferKUnitOfWork : UnitOfWork,ITransferKUnitOfWorkAsync
    {
        public TransferKUnitOfWork(ITransferKDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
