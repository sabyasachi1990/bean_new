using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CashSalesModule.Entities.V2;
using Service.Pattern;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public interface IItemCompactService : IService<ItemCompact>
    {
        List<ItemCompact> GetAllItemById(List<Guid?> id, long companyId);
    }
}
