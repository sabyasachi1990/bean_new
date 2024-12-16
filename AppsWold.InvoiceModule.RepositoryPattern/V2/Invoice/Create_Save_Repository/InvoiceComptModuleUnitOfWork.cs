using Repository.Pattern.Ef6;

namespace AppsWorld.InvoiceModule.RepositoryPattern.V2
{
    public class InvoiceComptModuleUnitOfWork : UnitOfWork,IInvoiceComptModuleUnitOfWorkAsync
    {
        public InvoiceComptModuleUnitOfWork(IInvoiceComptModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
