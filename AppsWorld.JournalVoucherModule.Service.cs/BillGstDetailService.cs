using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using System;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    //public class BillGstDetailService : Service<BillGSTDetail>, IBillGstDetailService
    //{
    //    private readonly IJournalVoucherModuleRepositoryAsync<BillGSTDetail> _billGstDetailrepository;
    //    public BillGstDetailService (IJournalVoucherModuleRepositoryAsync<BillGSTDetail> billGstDetailrepository):base(billGstDetailrepository)
    //    {
    //        _billGstDetailrepository = billGstDetailrepository;
    //    }
    //    public List<BillGSTDetail> GetAllGstBillDetailModel(Guid billId)
    //    {
    //        return _billGstDetailrepository.Query(x => x.BillId == billId).Select().ToList();
    //    }
    //    public BillGSTDetail GetGstBillDetailById(Guid Id, Guid Billid)
    //    {
    //        return _billGstDetailrepository.Query(x => x.Id == Id && x.BillId == Billid).Select().FirstOrDefault();

    //    }

         
    //}
}
