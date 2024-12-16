using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        private readonly IBankTransferModuleRepositoryAsync<Invoice> _invoiceRepository;
        public InvoiceService(IBankTransferModuleRepositoryAsync<Invoice> invoiceRepository) : base(invoiceRepository)
        {
            this._invoiceRepository = invoiceRepository;
        }
        public List<Invoice> GetListOfInvoice(long companyId, long serviceEntityId, Guid entityId, DateTime transferDate, string currency)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceEntityId && a.EntityId == entityId && a.DocDate <= transferDate && a.DocumentState != "Void" && a.Nature == "Interco" && a.DocCurrency == currency && a.DocType == "Invoice").Select().ToList();
        }
        public List<Invoice> GetListOfInvoicesByCompanyIdAndDocId(long companyId, List<Guid> docIds)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && docIds.Contains(a.Id) && a.DocumentState != "Void" && a.DocType == "Invoice").Select().ToList();
        }
        public List<Invoice> GetListOfICInvoiceBySEIdandEntId(long companyId, List<long?> lstServiceEntityIds, List<Guid> lstEntityIds, DateTime transferDate, string currency)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && lstServiceEntityIds.Contains(a.ServiceCompanyId.Value) && lstEntityIds.Contains(a.EntityId) && a.DocDate <= transferDate && a.DocumentState != "Void" && a.Nature == "Interco" && a.DocCurrency == currency && a.DocType == "Invoice").Select().ToList();
        }
    }
}
