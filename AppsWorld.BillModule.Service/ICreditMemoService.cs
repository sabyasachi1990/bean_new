using AppsWorld.BillModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public interface ICreditMemoService:IService<CreditMemo>
    {
        CreditMemo GetCreditMemoByCompanyId(long companyId);
        CreditMemo GetDocNo(string docNo, long companyId);
        CreditMemo GetCmById(Guid? id, long companyId);
        CreditMemo GetLastCreditMemo(long companyId);
    }
}
