using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.RepositoryPattern
{
    public class CreditMemoModuleRepository<TEntity> : Repository<TEntity>, ICreditMemoModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public CreditMemoModuleRepository(ICreditMemoModuleDataContextAsync dataContext, ICreditMemoModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork)
       {
       }
    }
}
