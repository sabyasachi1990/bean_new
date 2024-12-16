using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.Models;
using Service.Pattern;
using System;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface IInvoiceKService : IService<InvoiceK>
    {
        IQueryable<InvoiceModelK> GetAllInvoicesK(string username, long companyId, string internalState);
        IQueryable<CreditNoteModelK> GetAllCreditNoteK(string username, long companyId);
        IQueryable<DoubtfullDebitModelK> GetAllDebitfulldebitK(string username, long companyId);
        IQueryable<RecurringInvoiceK> GetAllRecurringInvoicesK(string username, long companyId);
        IQueryable<InvoiceModelK> GetAllRecuurringPostedInvoicesK(long companyId, string internalState, Guid id);
         
    }
}
