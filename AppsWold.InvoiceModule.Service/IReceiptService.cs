using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IReceiptService : IService<Receipt>
    {
        Receipt GetReceipt(Guid id, long companyId);

        Receipt GetAllReceipts(long CompanyId);
        Receipt GetDocNo(string docNo, long companyId);
        List<Receipt> GetAllReceiptByEntity(Guid? entityId);
    }
}
