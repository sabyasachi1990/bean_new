using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace AppsWorld.ReminderModule.RepositoryPattern.V2Repository
{
    public class ReminderKModuleRepository<TEntity> : Repository<TEntity>, IReminderKModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ReminderKModuleRepository(IReminderKModuleDataContextAsync dataContext, IReminderKModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
