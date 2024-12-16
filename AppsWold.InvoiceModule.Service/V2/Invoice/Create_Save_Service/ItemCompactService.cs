using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using AppsWorld.InvoiceModule.Entities.V2;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class ItemCompactService : Service<ItemCompact>, IItemCompactService
    {
        private readonly IInvoiceComptModuleRepositoryAsync<ItemCompact> _itemRepository;

        public ItemCompactService(IInvoiceComptModuleRepositoryAsync<ItemCompact> itemRepository)
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
