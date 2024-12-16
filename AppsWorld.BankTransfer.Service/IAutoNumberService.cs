
using AppsWorld.BankTransferModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IAutoNumberService : IService<AutoNumber>
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        string GenerateAutoNumberForType(long companyId, string Type, List<string> lstBill, string companyCode, string serviceGroupCode);
        string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, List<string> lstBill, string Companycode = null, string ServiceGroup = null);
        bool? GetAutoNumberFlag(long companyId, string entityType);
    }
}
