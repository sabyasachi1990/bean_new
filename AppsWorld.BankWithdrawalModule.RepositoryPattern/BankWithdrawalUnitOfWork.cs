using Repository.Pattern.Ef6;

namespace AppsWorld.BankWithdrawalModule.RepositoryPattern
{
    public class BankWithdrawalUnitOfWork : UnitOfWork, IBankWithdrawalModuleUnitOfWorkAsync
    {
         public BankWithdrawalUnitOfWork(IBankWithdrawalModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
