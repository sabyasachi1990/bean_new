using AppsWorld.BankTransferModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IInvoiceService : IService<Invoice>
    {
        List<Invoice> GetListOfInvoice(long companyId, long serviceEntityId, Guid entityId, DateTime transferDate, string currency);
        List<Invoice> GetListOfInvoicesByCompanyIdAndDocId(long companyId, List<Guid> docIds);
        //recentlly modifed based on modified docs
        List<Invoice> GetListOfICInvoiceBySEIdandEntId(long companyId, List<long?> lstServiceEntityIds, List<Guid> lstEntityIds, DateTime transferDate, string currency);
    }
}
