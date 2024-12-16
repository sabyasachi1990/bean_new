using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Repository.Pattern.Ef6;

namespace AppsWorld.BankReconciliationModule.RepositoryPattern
{
    public class BankReconciliationModuleUnitOfWork : UnitOfWork, IBankReconciliationModuleUnitOfWorkAsync
    {
        public BankReconciliationModuleUnitOfWork(IBankReconciliationModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
