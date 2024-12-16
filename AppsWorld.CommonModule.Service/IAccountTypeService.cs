using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.CommonModule.Service
{
    public interface IAccountTypeService : IService<AccountType>
    {
        AccountType GetById(long companyId, string name);
        AccountType GetAccountTypeId(long companyId, string name);
        List<AccountType> GetAllAccounyTypeByName(long companyId, List<string> name);
        List<long> GetAllAccounyTypeByNameByID(long companyId, List<string> name);
        Task<List<AccountType>> GetAllAccounyType(long companyId);
        List<AccountType> GetAllAccountTypeNameByCompanyId(long companyId, List<string> name);
        List<AccountType> GetAllAccounyTypesForClearing(long companyId, List<string> classes);
        List<AccountType> GetAllAccountType(long companyId, List<long> lstAccId);
        AccountType GetCashBankAccountbyName(long companyId, string name);
        List<AccountType> GetLeadSheetByCid(long companyid, bool v);
        AccountType GetAccountByName(long companyId, string name);
        Task<List<AccountType>> GetAllAccountTypeNameByCompanyIdAysnc(long companyId, List<string> name);
        Task<AccountType> GetByIdAsync(long companyId, string name);

    }
}
