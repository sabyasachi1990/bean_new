using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.CashSalesModule.RepositoryPattern.V2
{
    public interface ICashSalesKModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
}
