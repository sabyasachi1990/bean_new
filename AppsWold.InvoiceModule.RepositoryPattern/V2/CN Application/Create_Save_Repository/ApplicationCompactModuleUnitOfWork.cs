using Repository.Pattern.Ef6;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class ApplicationCompactModuleUnitOfWork : UnitOfWork, IApplicationCompactModuleUnitOfWorkAsync
    {
        public ApplicationCompactModuleUnitOfWork(IApplicationCompactModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
