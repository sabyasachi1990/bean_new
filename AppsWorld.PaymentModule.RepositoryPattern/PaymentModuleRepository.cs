using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.PaymentModule.RepositoryPattern
{
    public class PaymentModuleRepository<TEntity> : Repository<TEntity>, IPaymentModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public PaymentModuleRepository(IPaymentModuleDataContextAsync dataContext, IPaymentModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
