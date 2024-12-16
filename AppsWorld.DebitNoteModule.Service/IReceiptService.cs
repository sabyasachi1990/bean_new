using AppsWorld.DebitNoteModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IReceiptService : IService<Receipt>
    {
        Receipt GetReceipt(Guid id, long companyId);
        Receipt GetReceiptByComapnyId(long companyId, string docType);
        Receipt GetDuplicateReceipt(long companyId, string docType, string docNo);
        Receipt GetLastCreatedReceipt(long companyId);
    }
}
