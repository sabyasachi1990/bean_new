using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.BillModule.Infra;

namespace AppsWorld.PaymentModule.Service
{
    public class BillService : Service<Bill>, IBillService
    {
        private readonly IPaymentModuleRepositoryAsync<Bill> _billRepository;
        public BillService(IPaymentModuleRepositoryAsync<Bill> billRepository) : base(billRepository)
        {
            _billRepository = billRepository;
        }
        public List<string> GetByEntityId(Guid entityId, long companyId)
        {
            return _billRepository.Query(x => x.EntityId == entityId && x.CompanyId == companyId && (x.DocumentState == InvoiceState.NotPaid || x.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetByEntityIdState(Guid entityId, long companyId)
        {
            return _billRepository.Query(x => x.EntityId == entityId && x.CompanyId == companyId && x.DocumentState != InvoiceState.Void).Select(d => d.DocCurrency).ToList();
        }
        public Bill GetBillById(Guid id, long companyId)
        {
            return _billRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Bill GetBills(Guid id, string docSubType, string docCurrency)
        {
            return _billRepository.Query(x => x.Id == id && x.DocSubType == docSubType && x.DocCurrency == docCurrency && (x.DocumentState == BillNoteState.NotPaid || x.DocumentState == BillNoteState.PartialPaid) && x.GrandTotal != 0).Select().FirstOrDefault();
        }
        public List<Bill> GetBillByEntity(string docSubType, long companyId, Guid entityId, string docCurrency, long serviceCompanyId, DateTime? docDate)
        {
            //if (serviceCompanyId != null)
            //{
            //return _billRepository.Query(c => c.DocSubType == docSubType && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == docCurrency && c.ServiceCompanyId == serviceCompanyId && c.DocumentDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            if (docCurrency != null)
                return _billRepository.Query(c => (docSubType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == docCurrency && c.ServiceCompanyId == serviceCompanyId && c.PostingDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _billRepository.Query(c => (docSubType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") && c.CompanyId == companyId && c.EntityId == entityId && c.ServiceCompanyId == serviceCompanyId && c.PostingDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            // }
            //else
            //{
            //    return _billRepository.Query(c => c.DocSubType == docSubType && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == docCurrency && c.DocumentDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            //}

        }
        public List<Bill> InterCompany(string docSubType, long companyId, Guid entityId, string docCurrency, DateTime? docDate)
        {

            //return _billRepository.Query(c => c.DocSubType == docSubType && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == docCurrency && c.DocumentDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            if (docCurrency != null)
                return _billRepository.Query(c => (docSubType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") && c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == docCurrency && c.PostingDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _billRepository.Query(c => (docSubType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") && c.CompanyId == companyId && c.EntityId == entityId && c.PostingDate <= docDate && (c.DocumentState == BillNoteState.NotPaid || c.DocumentState == BillNoteState.PartialPaid) && c.GrandTotal != 0).Select().ToList();

        }
        public Bill GetBill(Guid id, string docSubType, string docCurrency, Guid entityId)
        {
            return _billRepository.Query(x => x.Id == id && x.DocSubType == docSubType && x.DocCurrency == docCurrency && x.EntityId == entityId).Select().FirstOrDefault();
        }
        public List<Bill> GetBill(List<Guid> billIds)
        {
            return _billRepository.Queryable().Where(c => billIds.Contains(c.Id)).ToList();
        }
        public List<string> GetAllCurrencyByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _billRepository.Query(x => x.EntityId == entityId && x.CompanyId == companyId && (x.DocCurrency == baseCurrency || x.DocCurrency == bankCurrency) && (x.DocumentState == InvoiceState.NotPaid || x.DocumentState == InvoiceState.PartialPaid)).Select(x => x.DocCurrency).ToList();
        }
        public List<string> GetAllCurrencyByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _billRepository.Query(x => x.EntityId == entityId && x.CompanyId == companyId && (x.DocCurrency == baseCurrency || x.DocCurrency == bankCurrency) && x.DocumentState != InvoiceState.Void).Select(x => x.DocCurrency).ToList();
        }
        public Bill GetBillsByDocId(Guid id, string docSubType, string docCurrency, Guid entityId)
        {
            return _billRepository.Query(x => x.Id == id && x.DocSubType == docSubType && x.DocCurrency == docCurrency && x.EntityId == entityId && x.DocumentState != BillNoteState.Void).Select().FirstOrDefault();
        }
        public List<Bill> GetAllBillsByDocId(List<Guid> docIds, List<string> docSubType, string docCurrency, Guid entityId)
        {
            return _billRepository.Query(x => docIds.Contains(x.Id) && docSubType.Contains(x.DocSubType) && x.DocCurrency == docCurrency && x.EntityId == entityId && x.DocumentState != BillNoteState.Void).Select().ToList();
        }
        public List<Bill> GetBillsByDocId(List<Guid> ids, long companyId)
        {
            return _billRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId).Select().ToList();
        }
        public decimal? GetExchangerateByDocId(long companyId, Guid docuemntId) => _billRepository.Query(a => a.CompanyId == companyId && a.Id == docuemntId).Select(a => a.ExchangeRate).FirstOrDefault();


        public List<Bill> GetAllBillsByDocumentId(List<Guid> docIds, List<string> docSubType, string docCurrency, Guid entityId)
        {
            return _billRepository.Query(x => docIds.Contains(x.Id) && docSubType.Contains(x.DocSubType) && x.DocCurrency == docCurrency && x.EntityId == entityId).Select().ToList();
        }
    }
}
