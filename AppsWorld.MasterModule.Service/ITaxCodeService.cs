using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.CommonModule.Infra;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ITaxCodeService : IService<TaxCode>
    {
        IQueryable<TaxCodeModelK> GetAllTaxCodeModelsK(long companyId, string jur);
        IEnumerable<TaxCode> GetAllCompanyById(long id, long CompanyId);
        IEnumerable<TaxCode> GetAllTaxCodeCodeAndCompanyId(string code, DateTime Datetime, long companyId);
        IEnumerable<TaxCode> GetAllTaxcodeCodeAndCompanyId(long Id, string Code, long CompanyId);
        List<TaxCode> GetAllTaxCodeByCompany(long companyId);
     
        List<LookUpCategory<string>> GetTaxCodeByIdandCid(long CompanyId, long? TaxId);
        List<LookUpCategory<string>> GetById(long CompanyId);
        IEnumerable<TaxCode> GetCodes(string code);
        TaxCode GetByTaxId(long? Id);
        List<TaxCodeLookUp<string>> GetAllTaxCode(long companyId);
        string GetTaxCode(long taxId, long companyId);
        List<EntityTaxCodeLookUp<string>> GetAllTaxCodes(long companyId, bool isEdit);
        List<TaxCode> GetTaxCode(long companyId);
        List<EntityTaxCodeLookUp<string>> GetAllTaxCodesBydocDate(long companyId, DateTime? docDate);
        List<TaxCode> GetTaxCodes(long companyId);
        List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date);

        Task<List<TaxCode>> GetTaxCodesAsync(long companyId);
        Task<List<TaxCode>> GetTaxCodeAsync(long companyId);
    }
}
