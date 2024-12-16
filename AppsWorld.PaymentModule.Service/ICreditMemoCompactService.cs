using AppsWorld.PaymentModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public interface ICreditMemoCompactService : IService<CreditMemoCompact>
    {
        List<CreditMemoCompact> GetListOfCreditMemos(long companyId, string docType, Guid entityId, List<Guid> lstDocumentIds);
        List<CreditMemoCompact> GetListOfCreditMemoWithOutInter(long companyId, long serviceCompanyId, string currency, Guid entityId, DateTime docDate);
        List<CreditMemoCompact> GetListOfCreditMemoWithInter(long companyId, string currency, Guid entityId, DateTime docDate);
        List<CreditMemoCompact> GetListOfCreditMemoCompacts(long companyId, List<Guid> ids);
        List<Guid?> GetListOfCreditMemoApps(long companyId, List<Guid> lstDocIds);
        List<CreditMemoCompact> GetAllCMsByDocId(List<Guid> docIds, string docCurrency, Guid entityId, long companyId);
        List<CreditMemoApplicationCompact> GetListOfCreditMemoAppsByCMIds(List<Guid> creditMemoIds, long companyId);
        List<CreditMemoCompact> GetAllCMByDocId(List<Guid> ids, long companyId);
        CreditMemoApplicationCompact GetCMApplicationByDocId(Guid detailId);
        decimal? GetExchangeRateByDocId(long companyId, Guid documentdId);
        List<string> GetByCreditMemoId(Guid entityId, long companyId);
        List<string> GetByStateandCreditMemoEntity(Guid entityId, long companyId);
        List<string> GetAllCreditMemoByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllCreditMemoEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
    }
}
