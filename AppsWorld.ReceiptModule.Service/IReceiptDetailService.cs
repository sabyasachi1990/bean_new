using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface IReceiptDetailService : IService<ReceiptDetail>
    {
        ReceiptDetail GetReceiptDetail(Guid id, Guid receiptId);
        List<ReceiptDetail> GetByReceiptId(Guid receiptId);
        List<ReceiptDetail> GetByReceiptIdSerId(Guid receiptId, long? serviceId, DateTime? docDate, string currency);
        List<ReceiptDetail> GetReceiptDetailById(Guid receiptId, DateTime? docDate, string currency);
        CreditNoteApplicationCompact GetCNApplicationByDocId(Guid detailId);
        CreditMemoApplicationCompact GetCMApplicationByDocId(Guid detailId);
    }
}
