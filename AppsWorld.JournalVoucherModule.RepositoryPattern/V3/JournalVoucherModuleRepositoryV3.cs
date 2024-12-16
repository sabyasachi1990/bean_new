using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.RepositoryPattern.V3
{
    public class JournalVoucherModuleRepositoryV3<TEntity> : Repository<TEntity>, IJournalVoucherModuleRepositoryAsyncV3<TEntity> where TEntity : class, IObjectState
    {
        public JournalVoucherModuleRepositoryV3(IJournalVoucherModuleDataContextAsyncV3 dataContext, IJournalVoucherModuleUnitOfWorkAsyncV3 unitOfWork)
           : base(dataContext, unitOfWork) { }
    }
}
