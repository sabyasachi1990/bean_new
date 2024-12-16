using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;

namespace AppsWorld.BankWithdrawalModule.RepositoryPattern
{
    public interface IBankWithdrawalModuleRepositoryAsync<TEntity>: IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
    
}
