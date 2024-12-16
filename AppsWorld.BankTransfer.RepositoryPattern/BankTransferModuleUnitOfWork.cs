using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.RepositoryPattern
{
    public class BankTransferModuleUnitOfWork:UnitOfWork,IBankTransferModuleUnitOfWorkAsync
    {
        public BankTransferModuleUnitOfWork(IBankTransferModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
