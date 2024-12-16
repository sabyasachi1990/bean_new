using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IReceiptDetailService : IService<ReceiptDetail>
    {
        List<ReceiptDetail> lstDetails(Guid DocumentId);
        List<ReceiptDetail> GetAllReceiptsByDocumentId(Guid DocumentId);
        List<PaymentDetailCompact> GetById(Guid bilid);
    }
}
