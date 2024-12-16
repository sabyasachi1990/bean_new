using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IAutoNumberCompanyService : IService<AutoNumberCompany>
    {
        List<AutoNumberCompany> GetAutoNumberCompany(Guid AutoNumberId);
    }
}
