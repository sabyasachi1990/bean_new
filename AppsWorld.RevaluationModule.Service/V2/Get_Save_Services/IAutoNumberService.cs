using AppsWorld.RevaluationModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public interface IAutoNumberService : IService<AutoNumberCompact>
    {
        AutoNumberCompact GetAutoNumber(long companyId, string entityType);
        bool? GetAutoNumberFlag(long companyId, string entityType);
        List<AutoNumberCompanyCompact> GetAutoNumberCompany(Guid AutoNumberId);
        AutoNumberCompanyCompact GetAutoCompany(Guid AutoNumberId);
        string GetAutoNumberPreview(long companyId, string entityType);

        string GetAutonumber(long companyId, string entityType, string connectionString);
    }
}
