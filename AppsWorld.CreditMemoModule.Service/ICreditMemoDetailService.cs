using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppsWorld.CreditMemoModule.Entities;

namespace AppsWorld.CreditMemoModule.Service
{
    public interface ICreditMemoDetailService:IService<CreditMemoDetail>
    {
        List<CreditMemoDetail> GetCreditMemoDetailById(Guid Id);
        CreditMemoDetail GetCreditMemoDetail(Guid id);
    }
}
