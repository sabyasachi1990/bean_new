using AppsWorld.OpeningBalancesModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public interface IOpeningBalanceDetailService : IService<OpeningBalanceDetail>
    {
        List<OpeningBalanceDetail> GetServiceCompanyOpeningBalance(Guid openiningBalanceId);
        OpeningBalanceDetail CheckOpeningBalanceId(Guid id);
        List<OpeningBalanceDetail> openingBalanceDetailByCOAID(long COAId, Guid OpeningBalanceId);
        OpeningBalanceDetail openingBalanceDetail(long COAId);
        OpeningBalanceDetail GetOBDetail(Guid id, Guid openingBalanceId);
        List<OpeningBalanceDetail> GetOpeningBalanceDetail(Guid openiningBalanceId);
        List<OpeningBalanceDetail> GetAllOBDetailAndLineItmsById(List<Guid> openiningBalanceIds);
        
    }
}
