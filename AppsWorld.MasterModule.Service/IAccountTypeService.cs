using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IAccountTypeService : IService<AccountType>
    {
        IEnumerable<AccountType> GetAllAccountType(long AccTypeId, long CompanyId);

        IEnumerable<AccountType> GetAllAccountTypes(long companyId);

        IEnumerable<AccountType> GetAllAccountTypeByCidIssys(long companyId, bool IsSystem);
        IEnumerable<AccountType> GetAllAccountTypes();
        long GetAllAccounyTypeByName(long companyId, string name);
        
        AccountType GetAllAccounyType(long companyId);
        List<long> GetAllAccounyTypeByNameByCOA(long companyId, List<string> name);
        Guid? GetAllAccountTypeById(string name, long companyId);
        List<long> GetAllNameByAccountType(long companyId, List<string> name);
        string GetAccountTypeName(long accountTypeId);
        //List<AccountType> GetAllAccounyTypeByNames(long companyId, List<string> name);
        Dictionary<long, string> GetAllAccounyTypeIdNames(long companyId, List<string> name);
        Task<List<long>> GetAllNameByAccountTypeAsync(long companyId, List<string> name);
    }
}
