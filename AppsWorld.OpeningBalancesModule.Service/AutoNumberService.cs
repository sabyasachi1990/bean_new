using AppsWorld.OpeningBalancesModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.OpeningBalancesModule.Service
{
    public class AutoNumberService : Service<AutoNumber>/*Service<BeanAutoNumber>*/, IAutoNumberService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<AutoNumber> _autoNumberepository;
        //private readonly IOpeningBalancesModuleRepositoryAsync<BeanAutoNumber> _autoNumberepository;

        public AutoNumberService(IOpeningBalancesModuleRepositoryAsync<AutoNumber> autoNumberRepository/*IOpeningBalancesModuleRepositoryAsync<BeanAutoNumber> autoNumberRepository*/)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }

        //public BeanAutoNumber GetAutoNumber(long companyId, string entityType)
        //{
        //    return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        //}
    }
}
