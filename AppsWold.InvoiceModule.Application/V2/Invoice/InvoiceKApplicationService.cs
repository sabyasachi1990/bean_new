using System.Linq;
using AppsWorld.InvoiceModule.Service.V2;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.Infra.Resources;
using System;

namespace AppsWorld.InvoiceModule.Application.V2
{
    public class InvoiceKApplicationService
    {
        IInvoiceKService _invoiceService;
        public InvoiceKApplicationService(IInvoiceKService invoiceService)
        {
            this._invoiceService = invoiceService;
        }

        #region Invoice_Kendo_Block
        public IQueryable<InvoiceModelK> GetAllInvoicesK(string username, long companyId)
        {
            return _invoiceService.GetAllInvoicesK(username, companyId, InvoiceState.Posted);
        }
        public IQueryable<InvoiceModelK> GetAllParkedInvoicesK(string username, long companyId)
        {
            return _invoiceService.GetAllInvoicesK(username, companyId, InvoiceState.Parked);
        }
        public IQueryable<RecurringInvoiceK> GetAllRecurringInvoicesK(string username, long companyId)
        {
            return _invoiceService.GetAllRecurringInvoicesK(username, companyId);
        }
        public IQueryable<InvoiceModelK> GetAllRecurringPostedInvoicesK(long companyId, Guid id)
        {
            return _invoiceService.GetAllRecuurringPostedInvoicesK(companyId, InvoiceState.Posted, id);
        }
        #endregion Invoice_Kendo_Block

        #region CreditNote_Kendo_Block
        public IQueryable<CreditNoteModelK> GetAllCreditNoteK(string username, long companyId)
        {
            return _invoiceService.GetAllCreditNoteK(username, companyId);
        }
        #endregion CreditNote_Kendo_Block

        #region Debit_Provision_Kendo_Block
        public IQueryable<DoubtfullDebitModelK> GetAllDebitfulldebitK(string username, long companyId)
        {
            return _invoiceService.GetAllDebitfulldebitK(username, companyId);
        }
        #endregion Debit_Provision_Kendo_Block
    }
}
