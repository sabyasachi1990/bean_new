using AppsWorld.BankTransferModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IBillService : IService<Bill>
    {
        List<Bill> GetListOfBill(long companyId, long serviceEntityId, Guid entityId, DateTime transferDate, string currency);
        List<Bill> GetListOfBillsByCompanyIdAndDocId(long companyId, List<Guid> docIds);

        //to Get list of Bills by invoice Ids to update teh state along with the Invoice
        List<Bill> GetListOfBillsByInvoiceIds(long companyId, List<Guid> lstOfInvoiceIds);
    }
}
