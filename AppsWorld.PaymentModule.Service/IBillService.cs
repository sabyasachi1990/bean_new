using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.PaymentModule.Entities;

namespace AppsWorld.PaymentModule.Service
{
    public interface IBillService : IService<Bill>
    {
        List<string> GetByEntityId(Guid entityId, long companyId);
        Bill GetBillById(Guid id, long companyId);
        Bill GetBills(Guid id, string docType, string docCurrency);
        List<Bill> GetBillByEntity(string docType, long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate);
        Bill GetBill(Guid id, string docSubType, string docCurrency, Guid entityId);
        List<Bill> InterCompany(string docSubType, long companyId, Guid entityId, string docCurrency, DateTime? docDate);
        List<Bill> GetBill(List<Guid> billIds);
        List<string> GetAllCurrencyByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetByEntityIdState(Guid entityId, long companyId);
        List<string> GetAllCurrencyByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        Bill GetBillsByDocId(Guid id, string docSubType, string docCurrency, Guid entityId);
        List<Bill> GetAllBillsByDocId(List<Guid> docIds, List<string> docSubType, string docCurrency, Guid entityId);
        List<Bill> GetBillsByDocId(List<Guid> ids, long companyId);
        decimal? GetExchangerateByDocId(long companyId, Guid docuemntId);

        //New services for payment
        List<Bill> GetAllBillsByDocumentId(List<Guid> docIds, List<string> docSubType, string docCurrency, Guid entityId);

    }
}
