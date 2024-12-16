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
    public interface IBillService : IService<Bill>
    {
        List<Bill> GetAllBillModel(long companyId);
        Bill GetBillById(Guid id, long companyId);
        Bill CreateBill(long companyId);
        Bill GetDocNo(string docNo, long companyId);
        Bill GetByAllBillDetails(Guid id);
        void BillUpdate (Bill bill);
        void BillInsert(Bill bill);

        Bill GetDocNoById(Guid Id, long companyId);

        Bill GetByDocTypeId(Guid id, string DocType, long companyId);
        Bill GetAllBillBuId(Guid Id);
        OpeningBalance GetOBByServiceCompanyId(long companyId, long? serviceCompanyId, string docType);
    }
}
