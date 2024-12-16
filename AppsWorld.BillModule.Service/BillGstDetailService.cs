using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using System;

namespace AppsWorld.BillModule.Service
{
    //public class BillGstDetailService : Service<BillGSTDetail>, IBillGstDetailService
    //{
    //    private readonly IBillModuleRepositoryAsync<BillGSTDetail> _billGstDetailrepository;
    //    public BillGstDetailService (IBillModuleRepositoryAsync<BillGSTDetail> billGstDetailrepository):base(billGstDetailrepository)
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
