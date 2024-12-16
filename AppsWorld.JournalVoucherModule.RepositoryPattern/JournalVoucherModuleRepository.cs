using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.JournalVoucherModule.RepositoryPattern
{
    public class JournalVoucherModuleRepository<TEntity> : Repository<TEntity>, IJournalVoucherModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public JournalVoucherModuleRepository(IJournalVoucherModuleDataContextAsync dataContext, IJournalVoucherModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
