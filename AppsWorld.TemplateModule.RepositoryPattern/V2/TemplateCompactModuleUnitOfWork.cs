
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.DataContext;

namespace AppsWorld.TemplateModule.RepositoryPattern.V2
{
    public class TemplateCompactModuleUnitOfWork : UnitOfWork, ITemplateCompactModuleUnitOfWorkAsync
    {
        public TemplateCompactModuleUnitOfWork(ITemplateModuleDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
