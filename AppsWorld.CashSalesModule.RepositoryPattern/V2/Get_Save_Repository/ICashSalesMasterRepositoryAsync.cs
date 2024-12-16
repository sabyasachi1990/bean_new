using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public interface ICashSalesMasterRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
}
