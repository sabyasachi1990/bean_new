using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.RepositoryPattern
{
    public class ClearingModuleUnitOfWork : UnitOfWork, IClearingModuleUnitOfWorkAsync
    {
        public ClearingModuleUnitOfWork(IClearingModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
