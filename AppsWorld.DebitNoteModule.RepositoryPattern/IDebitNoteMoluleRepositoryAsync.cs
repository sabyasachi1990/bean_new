using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;

namespace AppsWorld.DebitNoteModule.RepositoryPattern
{
    public interface IDebitNoteMoluleRepositoryAsync<TEntity>:IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
}
