using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReminderModule.RepositoryPattern
{
    public class ReminderModuleRepository<TEntity> : Repository<TEntity>, IReminderModuleRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public ReminderModuleRepository(IReminderModuleDataContextAsync dataContext, IReminderModuleUnitOfWorkAsync unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
