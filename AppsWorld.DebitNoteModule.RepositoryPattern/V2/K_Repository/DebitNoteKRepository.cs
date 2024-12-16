using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public class DebitNoteKRepository<TEntity> : Repository<TEntity>, IDebitNoteKRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public DebitNoteKRepository(IDebitNoteKDataContextAsync dataContext, IDebitNoteKUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {

        }
    }
}
