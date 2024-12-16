using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.RepositoryPattern
{
    public class BankTransferModuleRepository<TEntity> : Repository<TEntity>,
        IBankTransferModuleRepositoryAsync<TEntity> where TEntity : class,IObjectState
    {
        public BankTransferModuleRepository(IBankTransferModuleDataContextAsync dataContext, IBankTransferModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
