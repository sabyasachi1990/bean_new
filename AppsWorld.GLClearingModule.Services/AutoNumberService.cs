using AppsWorld.GLClearingModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.GLClearingModule.Service
{
    public class AutoNumberService : Service<AutoNumber>,IAutoNumberService
    {
        private readonly IClearingModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(IClearingModuleRepositoryAsync<AutoNumber> autoNumberRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoById(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(z=>z.IsEditable).FirstOrDefault();
        }
    }
}
