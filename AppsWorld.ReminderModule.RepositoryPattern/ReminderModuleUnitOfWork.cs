using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.RepositoryPattern
{
    public class ReminderModuleUnitOfWork : UnitOfWork, IReminderModuleUnitOfWorkAsync
    {
        public ReminderModuleUnitOfWork(IReminderModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
