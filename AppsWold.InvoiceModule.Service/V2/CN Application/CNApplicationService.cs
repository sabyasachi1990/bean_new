using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class CNApplicationService : Service<CreditNoteApplication>, ICNApplicationService
    {
        private readonly IApplicationCompactModuleRepositoryAsync<CreditNoteApplication> _creditNoteApplicationRepository;
        private readonly IApplicationCompactModuleRepositoryAsync<CreditNoteApplicationDetail> _CNApplicationDetailRepository;
        private readonly IApplicationCompactModuleRepositoryAsync<InvoiceCompact> _invoiceRepository;
        private readonly IApplicationCompactModuleRepositoryAsync<DebitNoteCompact> _debitNoteRepository;

        public CNApplicationService(IApplicationCompactModuleRepositoryAsync<CreditNoteApplication> creditNoteApplicationRepository, IApplicationCompactModuleRepositoryAsync<CreditNoteApplicationDetail> CNApplicationDetailRepository, IApplicationCompactModuleRepositoryAsync<InvoiceCompact> invoiceRepository, IApplicationCompactModuleRepositoryAsync<DebitNoteCompact> debitNoteRepository)
            : base(creditNoteApplicationRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._creditNoteApplicationRepository = creditNoteApplicationRepository;
            _invoiceRepository = invoiceRepository;
            _debitNoteRepository = debitNoteRepository;
            _CNApplicationDetailRepository = CNApplicationDetailRepository;
        }
        public CreditNoteApplication GetAllCreditNote(Guid creditNoteId, Guid cnApplicationId, long companyId)
        {
            return _creditNoteApplicationRepository.Query(c => c.InvoiceId == creditNoteId && c.Id == cnApplicationId && c.CompanyId == companyId).Include(c => c.CreditNoteApplicationDetails).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetAllCreditNoteApplication(Guid cnApplicationId, long companyId)
        {
            return _creditNoteApplicationRepository.Query(c => c.Id == cnApplicationId && c.CompanyId == companyId).Include(c => c.CreditNoteApplicationDetails).Select().FirstOrDefault();
        }
        public List<CreditNoteApplicationDetail> GetCreditNoteDetail(Guid CreditNoteApplicationId)
        {
            return _CNApplicationDetailRepository.Query(c => c.CreditNoteApplicationId == CreditNoteApplicationId && c.CreditAmount > 0).Select().ToList();
        }
        public CreditNoteApplication GetCreditNoteByCompanyId(long companyId)
        {
            return _creditNoteApplicationRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetCreditNoteById(Guid id)
        {
            return _creditNoteApplicationRepository.Query(c => c.InvoiceId == id).Select(/*x => new { CNAppNo = x.CreditNoteApplicationNumber, Status = x.Status, Date = x.CreatedDate }*/).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public CreditNoteApplication GetCreditNoteByIds(Guid Id)
        {
            return _creditNoteApplicationRepository.Query(e => e.Id == Id).Include(a => a.CreditNoteApplicationDetails).Select().FirstOrDefault();
        }
        public void Insert(CreditNoteApplicationDetail detail)
        {
            _CNApplicationDetailRepository.Insert(detail);
        }
        public void Update(CreditNoteApplicationDetail detail)
        {
            _CNApplicationDetailRepository.Update(detail);
        }
        public InvoiceCompact GetCreditNoteByCompanyIdAndId(long companyid, Guid id)
        {
            return _invoiceRepository.Query(e => e.CompanyId == companyid && e.Id == id && e.DocType == DocTypeConstants.CreditNote && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<InvoiceCompact> GetAllDDByInvoiceId(List<Guid> Ids)
        {
            return _invoiceRepository.Queryable().Where(c => Ids.Contains(c.Id) && c.Status == RecordStatusEnum.Active).ToList();
        }
        public List<InvoiceCompact> GetAllCreditNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date)
        {
            return _invoiceRepository.Query(c => c.DocType == DocTypeConstants.Invoice && c.CompanyId == companyId && c.EntityId == EntityId && c.DocDate <= date && c.DocCurrency == DocCurrency && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) && c.InternalState == "Posted").Select().ToList();
        }
        public List<InvoiceCompact> GetAllInvoiceByEntityId(long companyId, Guid EntityId, string DocCurrency, DateTime date)
        {
            return _invoiceRepository.Query(c => c.DocType == DocTypeConstants.Invoice && c.CompanyId == companyId && c.EntityId == EntityId && c.DocDate <= date && c.DocCurrency == DocCurrency && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) && c.InternalState == "Posted").Select().ToList();
        }
        public InvoiceCompact GetinvoiceById(Guid Id)
        {
            return _invoiceRepository.Query(c => c.Id == Id && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public Dictionary<Guid, string> GetTaggedInvoicesByCustomerAndCurrency(Guid customerId, string currency, long companyId)
        {
            return _invoiceRepository.Query(a => a.EntityId == customerId && a.DocCurrency == currency && a.CompanyId == companyId && a.AllocatedAmount > 0).Select(c => new { Id = c.Id, DocNo = c.DocNo }).ToDictionary(Id => Id.Id, DocNo => DocNo.DocNo);
        }
        public string GetCNNextNo(Guid id)
        {
            return _invoiceRepository.Query(e => e.Id == id && e.DocType == DocTypeConstants.CreditNote).Select(c => c.DocNo).FirstOrDefault();
        }
        public void UpdateInvoice(InvoiceCompact invoice)
        {
            _invoiceRepository.Update(invoice);
        }
        public List<DebitNoteCompact> GetDDByDebitNoteId(List<Guid> Ids)
        {
            return _debitNoteRepository.Queryable().Where(c => Ids.Contains(c.Id)).ToList();
        }
        public List<DebitNoteCompact> GetAllDebitNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docdate)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == EntityId && c.DocCurrency == DocCurrency && c.DocDate <= docdate && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }
        public List<DebitNoteCompact> GetAllDebitNoteByEntityId(long companyId, Guid EntityId, string DocCurrency, DateTime docdate)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == EntityId && c.DocCurrency == DocCurrency && c.DocDate <= docdate && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }
        public Dictionary<Guid, string> GetTaggedDebitNotesByCustomerAndCurrency(Guid customerId, string currency, long companyId)
        {
            return _debitNoteRepository.Query(a => a.EntityId == customerId && a.DocCurrency == currency && a.CompanyId == companyId && a.AllocatedAmount > 0).Select(c => new { Id = c.Id, DocNo = c.DocNo }).ToDictionary(Id => Id.Id, DocNo => DocNo.DocNo);
        }
        public void UpdateDebitNote(DebitNoteCompact debitNote)
        {
            _debitNoteRepository.Update(debitNote);
        }
        public DebitNoteCompact GetDebitNoteById(Guid Id)
        {
            return _debitNoteRepository.Query(c => c.Id == Id && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _invoiceRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == InvoiceStates.Void).FirstOrDefault() == true;
        }

        public List<DebitNoteCompact> GetAllDebitNotesById(long companyId, Guid entityId, string docCurrency, long ServiceCompanyId, DateTime applicationDate)
        {
            return _debitNoteRepository.Query(c => c.DocSubType == DocTypeConstants.DebitNote && c.Nature==DocTypeConstants.Interco && c.CompanyId == companyId && c.EntityId == entityId && c.DocDate <= applicationDate && c.DocCurrency == docCurrency && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) /*&& c.InternalState == "Posted"*/).Select().ToList();
        }
    }
}
