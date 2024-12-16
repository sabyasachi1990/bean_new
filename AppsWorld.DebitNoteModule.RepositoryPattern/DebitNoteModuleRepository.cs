using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.DebitNoteModule.RepositoryPattern
{
    public class DebitNoteModuleRepository<TEntity> : Repository<TEntity>, IDebitNoteMoluleRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public DebitNoteModuleRepository(IDebitNoteModuleDataContextAsync dataContext, IDebitNoteModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
        {

        }
    }
}
