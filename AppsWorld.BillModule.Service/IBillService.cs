using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Models;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
    public interface IBillService : IService<Bill>
    {
        List<Bill> GetAllBillModel(long companyId);
        Bill GetBillById(Guid id, long companyId, string docType);
        Bill CreateBill(long companyId);
        Bill GetDocNo(string docNo, long companyId);
        Bill GetByAllBillDetails(Guid id, string docType);
        void BillUpdate(Bill bill);
        void BillInsert(Bill bill);

        Bill GetDocNoById(Guid Id, long companyId);

        bool GetByDocTypeId(Guid id, string docNo, long companyId, string docType, Guid entityId);
        bool GetByDocSubTypeId(Guid id, string docNo, long companyId, string docType, Guid entityId, string docSubType);
        IQueryable<Bill> GetAllBills(long companyId);
        List<Bill> GetAllPayrollBill(Guid? payrollId, long companyId);

        IQueryable<BillModelK> GetAllBillsK(string username, long companyId, string type);

        Bill GetbillById(Guid id);

        DateTime? GetLastBillCreatedDate(long companyId);
        Bill GetBillById(long companyId, Guid billId);
        bool? GetClaimsBill(Guid id, long? companyId);
        decimal? GetBillBalAmount(Guid billId, long companyId);

        List<Bill> GetListOfBill(long companyId);

        List<Bill> GetListOfBills(List<Guid> lstBillIds, long companyId, string docSubType);
        bool IsVoid(long companyId, Guid id);
    }
}
