using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
	public interface IItemService : IService<Item>
    {
        Item GetItemById(Guid id, long companyId);
        List<Item> GetAllItemById(List<Guid?> id, long companyId);
        Guid GetItemByObId(string code, long companyId);
        //List<string> GetAllInvoiceDocNo(long companyId);
        //IDictionary<string, Guid> GetAllBillDocNo(long companyId);
    }
}
