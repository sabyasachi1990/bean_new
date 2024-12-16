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
    public class BillService : Service<Bill>, IBillService
    {
        private readonly IBankTransferModuleRepositoryAsync<Bill> _billRepository;
        public BillService(IBankTransferModuleRepositoryAsync<Bill> billRepository) : base(billRepository)
        {
            this._billRepository = billRepository;
        }
        public List<Bill> GetListOfBill(long companyId, long serviceEntityId, Guid entityId, DateTime transferDate, string currency)
        {
            return _billRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceEntityId && a.EntityId == entityId && a.PostingDate <= transferDate && a.DocumentState != "Void" && a.Nature == "Interco" && a.DocCurrency == currency).Select().ToList();
        }
        public List<Bill> GetListOfBillsByCompanyIdAndDocId(long companyId, List<Guid> docIds)
        {
            return _billRepository.Query(a => a.CompanyId == companyId && docIds.Contains(a.Id) && a.DocumentState != "Void").Select().ToList();
        }
        public List<Bill> GetListOfBillsByInvoiceIds(long companyId, List<Guid> lstOfInvoiceIds)
        {
            return _billRepository.Query(a => a.CompanyId == companyId && lstOfInvoiceIds.Contains(a.PayrollId.Value) && a.DocumentState != "Void").Select().ToList();
        }
    }
}
