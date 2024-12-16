using Repository.Pattern.Ef6;

namespace AppsWorld.PaymentModule.RepositoryPattern
{
    public class PaymentModuleUnitOfWork : UnitOfWork, IPaymentModuleUnitOfWorkAsync
    {
        public PaymentModuleUnitOfWork(IPaymentModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
