using Repository.Pattern.Ef6;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public class DebitNoteUnitOfWork : UnitOfWork, IDebitNoteUnitOfWorkAsync
    {
        public DebitNoteUnitOfWork(IDebitNoteDataContextAsync dataContext)
            : base(dataContext)
        {

        }
    }
}
