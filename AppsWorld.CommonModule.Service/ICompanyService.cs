using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Entities.Models;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public interface ICompanyService : IService<Company>
    {
        //Company GetByNameByServiceCompany(long parentId);
        string GetByNameByServiceCompany(long parentId);

        List<Company> GetCompany(long companyId, long? companyIdCheck, string username = null);

        Company GetById(long id);

        List<Company> GetServiceCompany(long parentId, long companyId);

        Company GetCompanyByCompanyid(long companyId);
        string GetIdBy(long Id);
        string GetByName(long id);
        List<Company> GetlstCompany(long id);
        List<LookUpCompany<string>> Listofsubsudarycompany(string username, long companyId, long? subcompanyid);
        List<LookUpCompany<string>> GetSubCompany(string username, long companyId, long? subcompanyid);
        List<LookUp<string>> CashAndBankCurrency(long companyId, long? subcompanyId, string currency);

        List<LookUpCompany<string>> Listofsubsidarycompany(long companyId, long? subcompanyid, List<long> COAIds, Guid bankRecId, string userName);//added by lokanath
        List<LookUpCompany<string>> ListOfSubsudaryCompanyActiveInactive(long companyId, long? subcompanyid, Guid Id, List<long> COAIds, string userName);
        Dictionary<long, RecordStatusEnum> GetAllCompaniesStatus(List<long> Ids);
        CompanyUser GetCompanyUserByCid_User(string username, long companyId);
        List<string> GetServiceCompanyNameById(List<long> serviceCompIds);
        List<Company> GetAllCompanies(List<long> ids);

        //new code for all service entities name
        Dictionary<long, string> GetAllCompaniesName(List<long> Ids);
        Dictionary<long, string> GetAllCompaniesNameByParentId(long parentId, bool? isInterCompanyActivate, bool isEdit);
        Dictionary<long, string> GetAllSubCompanies(List<long> Ids, string username, long companyId);
        Dictionary<long, string> GetAllSubCompany(string username, long companyId);
        List<long> GetAllSubCompaniesId(string username, long companyId);
        bool? GetServiceCompanyStatusByUsername(long servId, long companyId, string username);
        bool GetPermissionBasedOnUser(long? serviceEntityId, long? companyId, string username);
       Task<List<LookUpCompany<string>>> ListOfSubsudaryCompanyActiveInactiveAsync(long companyId, long? subcompanyid, Guid Id, List<long> COAIds, string userName);
    }
}
