using AppsWorld.BankTransferModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface ISettlementDetailService : IService<SettlementDetail>
    {
        List<SettlementDetail> GetListOfSettlemetDetails(Guid transferId, long withdrawalServiceEntityId, long depositServiceEntityId, string currency, DateTime transferDate);
        List<SettlementDetail> GetListOfSettlementDetails(Guid transferId, List<Guid> lstOfSettlemntDetailids);
    }
}
