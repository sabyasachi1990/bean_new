using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
  public  class BillDetailService:Service<BillDetail>,IBillDetailService
    {
      private readonly IJournalVoucherModuleRepositoryAsync<BillDetail> _billDetailRepository;
      public BillDetailService(IJournalVoucherModuleRepositoryAsync<BillDetail>  billDetailRepository):base(billDetailRepository)
      {
          _billDetailRepository= billDetailRepository;
      }
      public List<BillDetail> GetAllBillDetailModel(Guid BillId)
      {
          return _billDetailRepository.Query(x=>x.BillId==BillId).Select().ToList();
      }
      public BillDetail CreateBillDetail(Guid billId, Guid billDetailId)
      {
          return _billDetailRepository.Query(x => x.Id == billDetailId && x.BillId == billId).Select().FirstOrDefault();
      }

        public BillDetail GetBillDetailByBillId(Guid billId)
        {
            return _billDetailRepository.Query(s => s.BillId == billId).Select().FirstOrDefault();
        }
    }
}
