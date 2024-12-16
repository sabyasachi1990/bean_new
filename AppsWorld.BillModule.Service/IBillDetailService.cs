using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
   public interface IBillDetailService:IService<BillDetail>
    {
       List<BillDetail> GetAllBillDetailModel(Guid BillId);
       BillDetail CreateBillDetail(Guid billId, Guid billDetailId);
        List<ReceiptDetailCompact> GetById(Guid receiptId);
    }
}
