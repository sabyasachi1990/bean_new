using AppsWorld.OpeningBalancesModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public interface IBillService : IService<Bill>
    {
        IDictionary<string, Guid> GetAllInvoiceDocNo(long companyId);
        IDictionary<Guid, string> GetAllInvoiceDocNos(long companyId);
        //IDictionary<string, Guid> GetAllBillDocNo(long companyId);
        List<Bill> GetAllBillDocNo(long companyId);
        IDictionary<Guid, string> GetAllCreditNoteDocNo(long companyId);
        IDictionary<Guid, string> GetAllCreditMemoDocNo(long companyId);
        List<CreditMemo> GetlistOfCreditMemoDocNos(long companyId);
    }
}
