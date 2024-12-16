using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public class DebitNoteRepository<TEntity> : Repository<TEntity>, IDebitNoteRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public DebitNoteRepository(IDebitNoteDataContextAsync dataContext, IDebitNoteUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {

        }
    }
}
