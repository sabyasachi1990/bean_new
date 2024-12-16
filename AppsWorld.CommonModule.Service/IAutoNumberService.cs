using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Service
{
    public interface IAutoNumberService : IService<AutoNumber>
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        string GenerateAutoNumberForType(long companyId, string Type, List<string> lstBill, string companyCode, string serviceGroupCode);
        string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, List<string> lstBill, string Companycode = null, string ServiceGroup = null);
        string GetAutonumber(long companyId, string entityType, string connectionString);
        bool? GetAutoNumberIsEditable(long companyId, string entityType);
        string GetAutonumberInAddMode(long companyId, string entityType, string connectionString);
    }
}
