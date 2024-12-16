using AppsWorld.TemplateModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service
{
    public interface IGenericTemplateService : IService<GenericTemplate>
    {
        GenericTemplate GetGenerictemplate(long companyId, string templateType);
    }
}
