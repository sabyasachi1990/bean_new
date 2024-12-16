using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.RepositoryPattern
{
    public class CashSalesUnitOfWork :UnitOfWork,ICashSalesModuleUnitOfWorkAsync
    {
        public CashSalesUnitOfWork(ICashSalesModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
