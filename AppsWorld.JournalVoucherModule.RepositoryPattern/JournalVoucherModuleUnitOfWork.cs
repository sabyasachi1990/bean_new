using Repository.Pattern.Ef6;

namespace AppsWorld.JournalVoucherModule.RepositoryPattern
{
    public class JournalVoucherModuleUnitOfWork : UnitOfWork, IJournalVoucherModuleUnitOfWorkAsync
    {
        public JournalVoucherModuleUnitOfWork(IJournalVoucherModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
