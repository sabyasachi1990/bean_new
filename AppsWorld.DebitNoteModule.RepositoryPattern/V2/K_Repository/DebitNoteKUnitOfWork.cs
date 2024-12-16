using Repository.Pattern.Ef6;

namespace AppsWorld.DebitNoteModule.RepositoryPattern.V2
{
    public class DebitNoteKUnitOfWork : UnitOfWork, IDebitNoteKUnitOfWorkAsync
    {
        public DebitNoteKUnitOfWork(IDebitNoteKDataContextAsync dataContext)
            : base(dataContext)
        {

        }
    }
}
