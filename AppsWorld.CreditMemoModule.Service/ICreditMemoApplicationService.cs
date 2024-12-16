using AppsWorld.CreditMemoModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface ICreditMemoApplicationService:IService<CreditMemoApplication>
    {
        List<CreditMemoApplication> GetAllMemoApplication(Guid creditMemoId);
        CreditMemoApplication GetAllCreditMemo(Guid creditMemoId,Guid cmApplicationId,long companyId);
        CreditMemoApplication GetAllCreditMemoApplication(Guid cmApplicationId, long companyId);
        CreditMemoApplication GetCreditMemoByCompanyId(long companyId);
        CreditMemoApplication GetCreditMemoById(Guid id);
        List<CreditMemoApplication> GetCreditMemoApp(Guid id);
        CreditMemoApplication GetCreditMemo(Guid id);
    }
}
