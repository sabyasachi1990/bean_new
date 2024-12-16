using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Service
{
    public interface ITaxCodeService:IService<TaxCode>
    {
        IEnumerable<TaxCode> GetTaxCodesById(long Id);
        List<TaxCode> GetAllTaxCode(long taxId, long companyId, DateTime date);
        TaxCode GetTaxCode(long taxId);
        List<TaxCode> GetTaxCodes(long companyId);
        List<TaxCodeLookUp<string>> Listoftaxes(DateTime? date, bool isEdit, long companyId);
        string GetTaxCode(long taxId, long companyId);

        Task<List<TaxCode>> GetListOfTaxCode(long companyId);
        List<TaxCodeLookUp<string>> EditTaxCodes(List<long?> Id);
        List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date);
        long GetTaxID(string code, long companyId);
        long GetTaxCodeByName(string code, long companyId);
        Dictionary<long, string> GetTaxCodes(List<long?> taxIds, long companyId);
        List<TaxCode> GetTaxAllCodesNew(long companyId, DateTime? date, List<long?> ids);
        List<TaxCode> GetTaxAllCodesByIds(List<long?> ids);

        Task<List<TaxCode>> GetTaxAllCodesAsync(long companyId, DateTime? date);
       
    }
}
