using Repository.Pattern.Ef6;

namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public class CashSalesMasterUnitOfWork :UnitOfWork,ICashSalesMasterUnitOfWorkAsync
    {
        public CashSalesMasterUnitOfWork(ICashSalesMasterDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
