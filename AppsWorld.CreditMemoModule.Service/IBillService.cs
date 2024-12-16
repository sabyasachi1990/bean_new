using AppsWorld.CreditMemoModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface IBillService : IService<Bill>
    {
        Bill GetCrediMemoByDocId(Guid id);
        List<Bill> GetAllCreditMemoById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date);
        Bill GetCrediMemoByEntity(Guid id);
        List<Bill> GetAllBills(List<Guid?> ids, long companyId);
        List<decimal?> GetBillStatusByIds(List<Guid> Ids);

        List<Bill> GetAllBillsByDocIds(List<Guid> ids, long companyId);
    }
}
