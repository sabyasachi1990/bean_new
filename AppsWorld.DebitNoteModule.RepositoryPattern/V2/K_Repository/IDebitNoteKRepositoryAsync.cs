using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public interface IDebitNoteKRepositoryAsync<TEntity>:IRepositoryAsync<TEntity> where TEntity:class,IObjectState{ }
}
