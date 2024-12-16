using AppsWorld.BankWithdrawalModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public interface IGSTDetailService:IService<GSTDetail>
    {
        List<GSTDetail> GetAllGstDetail(Guid id, string docType);
        GSTDetail GetGSTById(Guid id);
    }
}
