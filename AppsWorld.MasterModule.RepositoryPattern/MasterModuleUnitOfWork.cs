using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.RepositoryPattern
{
    public class MasterModuleUnitOfWork : UnitOfWork, IMasterModuleUnitOfWorkAsync
    {
        public MasterModuleUnitOfWork(IMasterModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
