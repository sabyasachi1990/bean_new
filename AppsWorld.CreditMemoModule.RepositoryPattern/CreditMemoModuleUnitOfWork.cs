using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.RepositoryPattern
{
    public class CreditMemoModuleUnitOfWork : UnitOfWork,ICreditMemoModuleUnitOfWorkAsync
    {
        public CreditMemoModuleUnitOfWork(ICreditMemoModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
