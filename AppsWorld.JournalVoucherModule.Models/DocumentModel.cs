using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class DocumentModel
    {
        public long? ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string DocDescription { get; set; }
        public string DocCurrency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? GstExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
        public string GstReportingCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public Nullable<System.DateTime> GstdurationFrom { get; set; }
        public Nullable<System.DateTime> GstDurationTo { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsGstSettings { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public string SegmentName1 { get; set; }
        public string SegmentName2 { get; set; }
        public bool? IsRecurringJournal { get; set; }
        public string RecurringJournalName { get; set; }
        public string Frequency { get; set; }
        public Nullable<DateTime> FrequencyEndDate { get; set; }
        public bool? IsAutoReversalJournal { get; set; }
        public Nullable<DateTime> ReversalDate { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //invoice
        public System.Guid? EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string CreditTermsName { get; set; }
        public string Nature { get; set; }
        public string PONo { get; set; }
        public bool? IsRepeatingInvoice { get; set; }
        public Nullable<int> RepEveryPeriodNo { get; set; }
        public string RepEveryPeriod { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool? IsNoSupportingDocs { get; set; }
        public virtual List<DocumentDetailModel> DocumentDetailModels { get; set; }
        public virtual List<InvoiceCreditNoteModel> InvoiceCreditNoteModels { get; set; }
        public virtual List<InvoiceDoubtFulDebitModel> InvoiceDoubtFulDebitModels { get; set; }
        public virtual JournalTaxDetailsModel JournalTaxDetailsModel { get; set; }
        public virtual JournalGSTModel JournalGSTDetailModel { get; set; }
        public virtual List<TaxDetailsModel> TaxDetails { get; set; }
        public List<JournalGSTTaxModel> JournalGSTTaxModel { get; set; }
        public decimal DoubtfulDebitTotalAmount { get; set; }
        public Nullable<DateTime> BankClearingDate { get; set; }
        public string ModeOfReceipt { get; set; }
        public string SystemRefNo { get; set; }
        public string DocState { get; set; }
        public string ReceiptrefNo { get; set; }
        public string CashAndBankAccount { get; set; }
        public bool? IsSegmentActive { get; set; }
        //public long COAId { get; set; }
        public string ReceiptApplicationCurrency { get; set; }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Guid? DocumentId { get; set; }
        public bool IsInterCompanyActive { get; set; }
        public bool? IsAllowableNonAllowable { get; set; }
        public DateTime? PostingDate { get; set; }
        public string ReversalDocFor { get; set; }
        public bool? IsDocFor { get; set; }
        public Guid? ReverseParentRefId { get; set; }
        public Guid? ReverseChildRefId { get; set; }
        public bool? IsTaxEnable { get; set; }
    }
}
