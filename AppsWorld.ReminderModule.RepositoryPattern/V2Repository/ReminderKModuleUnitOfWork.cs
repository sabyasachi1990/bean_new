using Repository.Pattern.Ef6;

namespace AppsWorld.ReminderModule.RepositoryPattern.V2Repository
{
    public class ReminderKModuleUnitOfWork : UnitOfWork, IReminderKModuleUnitOfWorkAsync
    {
        public ReminderKModuleUnitOfWork(IReminderKModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
