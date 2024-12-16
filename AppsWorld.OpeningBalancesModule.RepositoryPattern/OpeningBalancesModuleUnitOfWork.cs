using Repository.Pattern.Ef6;
using System;


namespace AppsWorld.OpeningBalancesModule.RepositoryPattern
{
   public class OpeningBalancesModuleUnitOfWork : UnitOfWork, IOpeningBalancesModuleUnitOfWorkAsync
    {
        public OpeningBalancesModuleUnitOfWork(IOpeningBalancesModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
