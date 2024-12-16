using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class Journal : Entity
    {
        public Journal()
        {
            this.JournalDetails = new List<JournalDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DocNo { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public string SystemReferenceNo { get; set; }
        public Nullable<bool> IsNoSupportingDocs { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string DocCurrency { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public bool? IsSegmentReporting { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public decimal? GSTTotalAmount { get; set; }
        public string DocumentState { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? IsGstSettings { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsAllowableNonAllowable { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<short> Version { get; set; }
        public Nullable<int> Status { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<bool> IsRecurringJournal { get; set; }
        public string RecurringJournalName { get; set; }
        public Nullable<int> FrequencyValue { get; set; }
        public string FrequencyType { get; set; }
        public Nullable<System.DateTime> FrequencyEndDate { get; set; }
        public Nullable<bool> IsAutoReversalJournal { get; set; }
        public Nullable<System.DateTime> ReversalDate { get; set; }
        public Nullable<System.Guid> ReverseParentRefId { get; set; }
        public Nullable<System.Guid> ReverseChildRefId { get; set; }
        public string CreationType { get; set; }
        public decimal? GrandDocDebitTotal { get; set; }
        public decimal? GrandDocCreditTotal { get; set; }
        public Nullable<decimal> GrandBaseDebitTotal { get; set; }
        public Nullable<decimal> GrandBaseCreditTotal { get; set; }
        public Nullable<bool> IsCopy { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string EntityType { get; set; }
        public string PoNo { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public Nullable<bool> IsGSTApplied { get; set; }
        public Nullable<bool> IsGSTDeRegistration { get; set; }
        public Nullable<System.DateTime> GSTDeRegistrationDate { get; set; }
        public Nullable<long> COAId { get; set; }
        public string ModeOfReceipt { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }
        public Nullable<decimal> BankReceiptAmmount { get; set; }
        public string BankChargesCurrency { get; set; }
        public Nullable<decimal> BankCharges { get; set; }
        public string ExcessPaidByClient { get; set; }
        public string ExcessPaidByClientCurrency { get; set; }
        public Nullable<decimal> ExcessPaidByClientAmmount { get; set; }
        public string BalancingItemReciveCRCurrency { get; set; }
        public Nullable<decimal> BalancingItemReciveCRAmount { get; set; }
        public string BalancingItemPayDRCurrency { get; set; }
        public Nullable<decimal> BalancingItemPayDRAmount { get; set; }
        public string ReceiptApplicationCurrency { get; set; }
        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }
        public Nullable<System.Guid> DocumentId { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public string ClearingStatus { get; set; }
        public string Nature { get; set; }
        public Nullable<bool> IsRepeatingInvoice { get; set; }
        public Nullable<int> RepEveryPeriodNo { get; set; }
        public string RepEveryPeriod { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<long> CreditTermsId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("JournalId")]
        public virtual ICollection<JournalDetail> JournalDetails { get; set; }
    }
}
