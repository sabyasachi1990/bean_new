using AppsWorld.InvoiceModule.RepositoryPattern;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWold.InvoiceModule.RepositoryPattern
{
    public class InvoiceModuleRepository<TEntity> : Repository<TEntity>, IInvoiceModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public InvoiceModuleRepository(IInvoiceModuleDataContextAsync dataContext, IInvoiceModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
       {
       }
    }
}
