using AppsWorld.CreditMemoModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface ICreditMemoApplicationDetailService : IService<CreditMemoApplicationDetail>
    {
        List<CreditMemoApplicationDetail> GetAllCreditMemoDetail(Guid creditMemoApplicationId);
        //List<CreditMemoApplicationDetail> GetCreditMemoDetailById(Guid documentId);
    }
}




