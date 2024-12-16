using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.ReceiptModule.RepositoryPattern.V2
{
    public class ReceiptKModuleRepository<TEntity> : Repository<TEntity>, IReceiptKModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ReceiptKModuleRepository(IReceiptKModuleDataContextAsync dataContext, IReceiptKModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
