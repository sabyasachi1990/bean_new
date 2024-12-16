using AppsWorld.InvoiceModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface IAutoNumberService : IService<AutoNumberCompact>
    {
        AutoNumberCompact GetAutoNumber(long companyId, string entityType);
        bool? GetAutoNumberFlag(long companyId, string entityType);
        List<AutoNumberComptCompany> GetAutoNumberCompany(Guid AutoNumberId);
    }
}
