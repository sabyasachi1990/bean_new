using Repository.Pattern.Ef6;

namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public class CashSalesKUnitOfWork :UnitOfWork,ICashSalesKModuleUnitOfWorkAsync
    {
        public CashSalesKUnitOfWork(ICashSalesKModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
