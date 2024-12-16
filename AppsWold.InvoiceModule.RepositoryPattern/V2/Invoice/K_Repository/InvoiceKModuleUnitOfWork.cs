using Repository.Pattern.Ef6;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class InvoiceKModuleUnitOfWork : UnitOfWork,IInvoiceKModuleUnitOfWorkAsync
    {
        public InvoiceKModuleUnitOfWork(IInvoiceKModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
