using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.InvoiceModule.Entities.V2;
using Service.Pattern;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface IItemCompactService : IService<ItemCompact>
    {
        List<ItemCompact> GetAllItemById(List<Guid?> id, long companyId);
    }
}
