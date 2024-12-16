using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public class CashSalesMasterRepository<TEntity> : Repository<TEntity>, ICashSalesMasterRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public CashSalesMasterRepository(ICashSalesMasterDataContextAsync dataContext, ICashSalesMasterUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
