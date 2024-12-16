using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.RepositoryPattern;

namespace AppsWorld.CommonModule.Service
{
    public class ItemService : Service<Item>, IItemService
    {
        private readonly ICommonModuleRepositoryAsync<Item> _itemRepository;
        //private readonly ICommonModuleRepositoryAsync<Bill> _billRepository;
        //private readonly ICommonModuleRepositoryAsync<Invoice> _invoiceRepository;
        public ItemService(ICommonModuleRepositoryAsync<Item> itemRepository)
            : base(itemRepository)
        {
            _itemRepository = itemRepository;
        }
        public Item GetItemById(Guid id, long companyId)
        {
            return _itemRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<Item> GetAllItemById(List<Guid?> id, long companyId)
        {
            return _itemRepository.Query(x => id.Contains(x.Id) && x.CompanyId == companyId).Select().ToList();
        }
        public Guid GetItemByObId(string code, long companyId)
        {
            return _itemRepository.Query(x => x.Code == "Opening Balance" && x.CompanyId == companyId).Select(x => x.Id).FirstOrDefault();
        }
        //public List<string> GetAllInvoiceDocNo(long companyId)
        //{
        //    return _invoiceRepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void").Select(c => c.DocNo).ToList();
        //}
        //public IDictionary<string,Guid> GetAllBillDocNo(long companyId)
        //{
        //    return _billRepository.Queryable().Where(c => c.CompanyId == companyId && c.DocumentState != "Void").ToDictionary(c => c.DocNo, value => value.EntityId);
        //}
    }
}
