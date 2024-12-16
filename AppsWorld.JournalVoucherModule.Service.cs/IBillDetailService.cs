using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
   public interface IBillDetailService:IService<BillDetail>
    {
       List<BillDetail> GetAllBillDetailModel(Guid BillId);
       BillDetail CreateBillDetail(Guid billId, Guid billDetailId);
        BillDetail GetBillDetailByBillId(Guid billId);


    }
}
