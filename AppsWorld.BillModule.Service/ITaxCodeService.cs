using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
	public interface ITaxCodeService : IService<TaxCode>
    {
        List<TaxCode> GetTaxCodeEdit(long id, long companyId, DateTime date);
        List<TaxCode> GetTaxCodeNew(long CompanyId, DateTime date);
        TaxCode GetTaxCode(long taxId);
        TaxCode GetTaxById(long? Id);
        TaxCode GetTaxiId(long? Id);
        List<TaxCodeLookUp<string>> Listoftaxes(DateTime? date, bool isEdit, long companyId);
        List<TaxCode> GetAllTaxs(long companyId);
        TaxCode GetTaxByCode(string taxCode);
        List<TaxCode> GetTaxCodes(long companyId);
        List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date);
        List<TaxCode> GetTaxAllCodesByIds(List<long?> ids);
    }
}
