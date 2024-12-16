using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service
{
    public class GenericTemplateService : Service<GenericTemplate>, IGenericTemplateService
    {
        private readonly
        ITemplateModuleRepositoryAsync<GenericTemplate> _genericTemplateRepository;
        public GenericTemplateService(ITemplateModuleRepositoryAsync<GenericTemplate> genericTemplateRepository):base(genericTemplateRepository)
        {
            _genericTemplateRepository = genericTemplateRepository;
        }

        public GenericTemplate GetGenerictemplate(long companyId, string templateType)
        {
            GenericTemplate GenericTemplate = _genericTemplateRepository.Query(b => b.CompanyId == companyId && b.TemplateType == templateType).Select().FirstOrDefault();
            return GenericTemplate;
        }
    }
}
