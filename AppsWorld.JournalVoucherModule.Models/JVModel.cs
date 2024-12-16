using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JVModel
    {
        public long? ServiceCompanyId { get; set; }
        public string ServiceCompanyName { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
		public string DocType { get; set; }
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
        public virtual InvoiceModel InvoiceModel { get; set; }
        public virtual DebitNoteModel DebitNoteModel { get; set; }
        public virtual CreditNoteModel CreditNoteModel { get; set; }
		public virtual DoubtfulDebtModel DoubtfulDebtModel { get; set; }
        public virtual List<JournalDetailsModel> JournalDetailsModel { get; set; }
        public virtual JournalTaxDetailsModel JournalTaxDetailsModel { get; set; }
		public virtual JournalGSTModel JournalGSTDetailModel { get; set; }
		public bool? IsNoSupportingDocument { get; set; }
		public bool? IsAllowableNonAllowable { get; set; }
        public string TransferRefNo { get; set; }

    }
}
