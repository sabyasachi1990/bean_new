using System.Collections.Generic;
using Service.Pattern;
using System;
using AppsWorld.JournalVoucherModule.Entities;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ITaxCodeService : IService<TaxCode>
    {
        List<TaxCode> GetTaxCodeEdit(long id, long CompanyId, DateTime date);
        List<TaxCode> GetTaxCodeNew(long CompanyId, DateTime date);

        TaxCode GetTaxById(long? Id);
        TaxCode GetTaxiId(long? Id);
        TaxCode GetTaxId(long? Id);
        List<TaxCode> GetListOfTaxCode(long companyId);
        Task<List<TaxCode>> GetTaxCodes(long companyId);
        List<TaxCode> GetTaxCodesByIds(List<long> Ids);
        Task<List<TaxCode>> GetTaxAllCodes(long companyId, DateTime? date);
        long GetTaxId(string code, long companyId);
    }
}
