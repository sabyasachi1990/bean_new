using AppsWorld.InvoiceModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWold.InvoiceModule.RepositoryPattern
{
    public class InvoiceModuleUnitOfWork : UnitOfWork,IInvoiceModuleUnitOfWorkAsync
    {
        public InvoiceModuleUnitOfWork(IInvoiceModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
