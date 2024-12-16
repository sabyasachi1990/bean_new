using AppsWorld.CashSalesModule.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CashSalesModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.CashSalesModule.Service
{
    public class AutoNumberService : Service<AutoNumber>,IAutoNumberService
    {
        private readonly ICashSalesModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(ICashSalesModuleRepositoryAsync<AutoNumber> autoNumberRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoNumberIsEditable(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(c=>c.IsEditable).FirstOrDefault();
        }
    }
}
