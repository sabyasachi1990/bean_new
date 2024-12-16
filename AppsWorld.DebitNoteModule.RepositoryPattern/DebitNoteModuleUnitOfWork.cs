using Repository.Pattern.Ef6;

namespace AppsWorld.DebitNoteModule.RepositoryPattern
{
    public class DebitNoteModuleUnitOfWork : UnitOfWork, IDebitNoteModuleUnitOfWorkAsync
    {
        public DebitNoteModuleUnitOfWork(IDebitNoteModuleDataContextAsync dataContext)
            : base(dataContext)
        {

        }
    }
}
