using AppsWorld.GLClearingModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Service
{
    public interface IAutoNumberService : IService<AutoNumber>
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        bool? GetAutoById(long companyId, string entityType);
    }
}
