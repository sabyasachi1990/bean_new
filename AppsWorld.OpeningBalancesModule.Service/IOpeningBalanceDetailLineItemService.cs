using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public interface IOpeningBalanceDetailLineItemService : IService<OpeningBalanceDetailLineItem>
    {
        List<OpeningBalanceDetailLineItem> GetLineItemsForCOA(long COAId, long ServiceCompanyId, string currency);

        OpeningBalanceDetailLineItem GetLineItemById(Guid id);

        List<OpeningBalanceDetailLineItem> GetLstLineItemById(Guid id);
        bool? IsLineItemExist(Guid lineItemId, Guid detailId);

        bool? IsLineItemDocDate(List<Guid> detailId, DateTime? obDate);

        List<Guid> GetListOfLineItemId(List<long> coaids, long serviceCompanyId);
        List<Guid> GetListOfLineItemId1(List<long> coaids, long serviceCompanyId, long companyId);
        List<Guid?> ListOfEntityIds(List<long> coaIds, long serviceCompanyId);
        List<OpeningBalanceDetailLineItem> GetListOfOBDLineItemByCoaId(List<long> coaIds, long serviceCompanyId);
        Dictionary<Guid, string> GetListOfDeleteTPOPlineItem(List<Guid> lstLineItemId, List<long> coaIds, long serviceCompanyId);
        List<OpeningBalanceDetailLineItem> GetListOfTPOPLineItemId(List<Guid> TPOPIds, long serviceCompanyId);
        IQueryable<OpeningBalanceLineItemModel> GetLineItemsForCOAs(long COAId, long ServiceCompanyId, string currency, long companyId);
        OpeningBalanceDetail GetOpeningBalanceDetail(Guid openingBalanceId, Guid openingBalanceDetailId, long coaId);
        int GetOpeningBalanceDetailRecOrder(Guid openingBalanceId);
    }
}
