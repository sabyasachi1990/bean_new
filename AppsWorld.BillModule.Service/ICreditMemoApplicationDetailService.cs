using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppsWorld.BillModule.Entities;

namespace AppsWorld.BillModule.Service
{
    public interface ICreditMemoApplicationDetailService:IService<CreditMemoApplicationDetail>
    {
        List<CreditMemoApplicationDetail> GetCreditMemoDetailById(Guid Id);
    }
}
