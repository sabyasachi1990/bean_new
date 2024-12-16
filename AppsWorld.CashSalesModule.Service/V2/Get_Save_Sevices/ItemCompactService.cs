using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using AppsWorld.CashSalesModule.Entities.V2;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public class ItemCompactService : Service<ItemCompact>, IItemCompactService
    {
        private readonly ICashSalesMasterRepositoryAsync<ItemCompact> _itemRepository;

        public ItemCompactService(ICashSalesMasterRepositoryAsync<ItemCompact> itemRepository)
            : base(itemRepository)
        {
            this._itemRepository = itemRepository;
        }
        public List<ItemCompact> GetAllItemById(List<Guid?> id, long companyId)
        {
            return _itemRepository.Query(x => id.Contains(x.Id) && x.CompanyId == companyId).Select().ToList();
        }
        //public Dictionary<Guid, string> GetListOfEntity(long companyId, List<Guid?> entityId)
        //{
        //    return _entityRepository.Query(a => a.CompanyId == companyId && entityId.Contains(a.Id)).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        //}
    }
}
