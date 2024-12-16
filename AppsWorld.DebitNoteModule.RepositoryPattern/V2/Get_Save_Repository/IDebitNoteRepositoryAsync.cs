using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public interface IDebitNoteRepositoryAsync<TEntity>:IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
}
