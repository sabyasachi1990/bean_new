using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IAccountTypeService : IService<AccountType>
    {
        IEnumerable<AccountType> GetAllAccountType(long AccTypeId, long CompanyId);

        IEnumerable<AccountType> GetAllAccountTypes(long companyId);

        IEnumerable<AccountType> GetAllAccountTypeByCidIssys(long companyId, bool IsSystem);
        IEnumerable<AccountType> GetAllAccountTypes();
        AccountType GetAllAccounyTypeByName(long companyId, string name);
       // List<AccountType> GetAllAccounyTypeByName(long companyId, List<string> name);
        List<long> GetAllAccounyTypeByName(long companyId, List<string> name);


    }
}
