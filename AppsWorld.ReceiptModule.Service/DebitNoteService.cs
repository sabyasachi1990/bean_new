using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
    public class DebitNoteService : Service<DebitNote>, IDebitNoteService
    {
        private readonly IReceiptModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        private readonly IReceiptModuleRepositoryAsync<BillCompact> _billRepository;
        private readonly IReceiptModuleRepositoryAsync<CreditMemoCompact> _creditMemoRepository;

        public DebitNoteService(IReceiptModuleRepositoryAsync<DebitNote> debitNoteRepository, IReceiptModuleRepositoryAsync<BillCompact> billRepository, IReceiptModuleRepositoryAsync<CreditMemoCompact> creditMemoRepository)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
            _billRepository = billRepository;
            _creditMemoRepository = creditMemoRepository;
        }

        public DebitNote GetDebitNote(Guid id, string docCurrency)
        {
            return _debitNoteRepository.Query(c => c.Id == id && c.DocCurrency == docCurrency).Select().FirstOrDefault();
        }

        public DebitNote GetDebitNotes(Guid id, string docCurrency)
        {
            return _debitNoteRepository.Query(c => c.Id == id && c.DocCurrency == docCurrency && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().FirstOrDefault();
        }

        public List<DebitNote> GetDebitNoteByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate)
        {
            if (doccurrency != null)
                return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
        }
        public List<string> GetByEntityId(Guid entityId, long companyId)
        {
            return _debitNoteRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();

        }
        public List<string> GetByIdState(Guid entityId, long companyId)
        {
            return _debitNoteRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocumentState != "Void").Select(c => c.DocCurrency).ToList();

        }
        public DebitNote getDebitNoteById(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public DebitNote GetDebitNoteById(Guid id)
        {
            return _debitNoteRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public DebitNote getDebitNoteeById(Guid id, long companyId, string currency, Guid entityId)
        {
            return _debitNoteRepository.Query(c => c.Id == id && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void)).Select().FirstOrDefault();
        }
        public List<DebitNote> GetAllDebitNoteById(List<Guid> ids, long companyId, string currency, Guid entityId)
        {
            return _debitNoteRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void)).Select().ToList();
        }
        public List<DebitNote> GetDebitNoteByEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate)
        {
            if (doccurrency != null)
                return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.DocDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
        }
        public List<DebitNote> GetListOfDebitNote(long companyid, List<Guid> lstDocumentId)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyid && lstDocumentId.Contains(a.Id)).Select().ToList();
        }
        public IQueryable<DebitNote> GetListOfDebitNoteNew(long companyid, List<Guid> lstDocumentId)
        {
            return _debitNoteRepository.Queryable().Where(a => a.CompanyId == companyid && lstDocumentId.Contains(a.Id)).AsQueryable();
        }
        public List<string> GetAllDNByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _debitNoteRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllDNByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _debitNoteRepository.Queryable().Where(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && c.DocumentState != InvoiceState.Void).Select(c => c.DocCurrency).ToList();
        }




        #region Bill_Service
        public List<BillCompact> GetBillByEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate)
        {
            if (doccurrency != null)
                return _billRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _billRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
        }
        public List<BillCompact> GetBillByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate)
        {
            if (doccurrency != null)
                return _billRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == serviceCompanyId && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
            else
                return _billRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.ServiceCompanyId == serviceCompanyId && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid) && c.GrandTotal != 0).Select().ToList();
        }
        public List<BillCompact> GetAllBillsById(List<Guid> ids, long companyId, string currency, Guid entityId)
        {
            return _billRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void) && c.Nature != "Interco").Select().ToList();
        }
        public List<BillCompact> GetAllBillsByDocId(List<Guid> ids, long companyId)
        {
            return _billRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId).Select().ToList();
        }
        public IQueryable<BillCompact> GetAllBillsByDocIdNew(List<Guid> ids, long companyId)
        {
            return _billRepository.Queryable().Where(c => ids.Contains(c.Id) && c.CompanyId == companyId).AsQueryable();
        }
        public decimal? GetBillExchangeRate(Guid docId, long companyId) => _billRepository.Query(c => c.Id == docId && c.CompanyId == companyId).Select(c => c.ExchangeRate).FirstOrDefault();

        public void Update(BillCompact bill)
        {
            _billRepository.Update(bill);
        }
        #endregion Bill_Service


        #region CM_Service
        public List<CreditMemoCompact> GetCMEntityAndDocdate(long companyId, Guid entityId, string doccurrency, DateTime? docDate)
        {
            if (doccurrency != null)
                return _creditMemoRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0).Select().ToList();
            else
                return _creditMemoRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.PostingDate <= docDate && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0).Select().ToList();
        }
        public List<CreditMemoCompact> GetCMByEntity(long companyId, Guid entityId, string doccurrency, long serviceCompanyId, DateTime? docDate)
        {
            if (doccurrency != null)
                return _creditMemoRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0).Select().ToList();
            else
                return _creditMemoRepository.Query(c => c.CompanyId == companyId && c.EntityId == entityId && c.ServiceCompanyId == serviceCompanyId && c.DocDate <= docDate && (c.DocumentState == InvoiceState.Not_Applied || c.DocumentState == InvoiceState.PartialApplied) && c.GrandTotal != 0).Select().ToList();
        }
        public List<CreditMemoCompact> GetAllCreditMemoById(List<Guid> ids, long companyId, string currency, Guid entityId)
        {
            return _creditMemoRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId && c.DocCurrency == currency && c.EntityId == entityId && (c.DocumentState != InvoiceState.Void)).Select().ToList();
        }
        public List<CreditMemoCompact> GetAllCMByDocId(List<Guid> ids, long companyId)
        {
            return _creditMemoRepository.Query(c => ids.Contains(c.Id) && c.CompanyId == companyId).Select().ToList();
        }
        public IQueryable<CreditMemoCompact> GetAllCMByDocIdNew(List<Guid> ids, long companyId)
        {
            return _creditMemoRepository.Queryable().Where(c => ids.Contains(c.Id) && c.CompanyId == companyId).AsQueryable();
        }
        public decimal? GetMemoExchangeRate(Guid docId, long companyId) => _creditMemoRepository.Query(c => c.Id == docId && c.CompanyId == companyId).Select(c => c.ExchangeRate).FirstOrDefault();
        public void Update(CreditMemoCompact creditMemo)
        {
            _creditMemoRepository.Update(creditMemo);
        }
        #endregion CM_Service
    }
}
