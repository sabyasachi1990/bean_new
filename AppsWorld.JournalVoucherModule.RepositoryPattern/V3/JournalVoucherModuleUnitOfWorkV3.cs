using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.RepositoryPattern.V3
{
    public class JournalVoucherModuleUnitOfWorkV3 : UnitOfWork, IJournalVoucherModuleUnitOfWorkAsyncV3
    {
        public JournalVoucherModuleUnitOfWorkV3(IJournalVoucherModuleDataContextAsyncV3 dataContext)
           : base(dataContext) { }
    }
}
