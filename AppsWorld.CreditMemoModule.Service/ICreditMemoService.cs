using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface ICreditMemoService : IService<CreditMemo>
    {
        CreditMemo GetCreditMemoByCompanyId(long companyId, Guid id);
        CreditMemo GetCreditMemoByCompanyId(long companyId);
        CreditMemo GetAllMemoByDoctypeAndCompanyId(string docType, long companyId);
        CreditMemo GetMemos(string strNewDocNo, string docType, long CompanyId);
        IQueryable<CreditMemoModelK> GetAllCreditMemoK(string username, long companyId);
        CreditMemo GetMemoLU(long companyId, Guid creditId);
        CreditMemo GetMemo(Guid creditMemoId);
        CreditMemo GetCreditMemoById(Guid id);
        CreditMemo CheckDocExists(Guid Id, string docType, string docNo, long companyId, Guid entityId);
        List<CreditMemo> GetCompanyIdAndDocType(long companyId);
        List<CreditMemo> GetTaggedMemoByCustomerAndCurrency(Guid customerId, string currency, long companyId);
        CreditMemo GetByCompanyId(long companyId);
        CreditMemo GetMemoByDocId(long companyId, Guid? docId);

        bool IsDocumentNumberExists(Guid Id, string docType, string docNo, long companyId, Guid entityId);

        bool IsVoid(long companyId, Guid id);
    }
}
