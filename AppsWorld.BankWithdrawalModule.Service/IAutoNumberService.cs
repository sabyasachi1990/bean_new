using AppsWorld.BankWithdrawalModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public interface IAutoNumberService : IService<AutoNumber>
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        bool? GetAutoNumberFlag(long companyId, string entityType);
    }
}
