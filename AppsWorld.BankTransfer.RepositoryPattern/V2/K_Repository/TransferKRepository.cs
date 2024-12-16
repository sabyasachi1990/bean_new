using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.BankTransferModule.RepositoryPattern.V2
{
    public class TransferKRepository<TEntity> : Repository<TEntity>,
        ITransferKRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public TransferKRepository(ITransferKDataContextAsync dataContext, ITransferKUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
