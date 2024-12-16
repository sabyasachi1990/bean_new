using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JVVModel
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public bool IsNoSupportingDocument { get; set; }
        public string DocType { get; set; }
		public string BaseCurrency { get; set; }
		public Guid DocumentId { get; set; }
        public string DocSubType { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string SystemReferenceNo { get; set; }
        public Nullable<bool> IsNoSupportingDocs { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string DocCurrency { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public bool IsSegmentReporting { get; set; }
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
        public decimal GSTTotalAmount { get; set; }
        public string DocumentState { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
        public string DocumentDescription { get; set; }
        public Nullable<bool> IsRecurringJournal { get; set; }
        public string RecurringJournalName { get; set; }
        public Nullable<int> FrequencyValue { get; set; }
        public string FrequencyType { get; set; }
        public Nullable<System.DateTime> FrequencyEndDate { get; set; }
        public Nullable<bool> IsAutoReversalJournal { get; set; }
        public Nullable<System.DateTime> ReversalDate { get; set; }
        public Nullable<Guid> ReverseParentRefId { get; set; }
        public Nullable<Guid> ReverseChildRefId { get; set; }
        public decimal GrandDocDebitTotal { get; set; }
        public decimal GrandDocCreditTotal { get; set; }
        public Nullable<decimal> GrandBaseDebitTotal { get; set; }
        public Nullable<decimal> GrandBaseCreditTotal { get; set; }
		public bool? IsRepeatingInvoice { get; set; }
		public int? RepEveryPeriodNo { get; set; }
		public string RepEveryPeriod { get; set; }
		public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        public string ClearingStatus { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = (RecordStatusEnum)value; }
        }
        public bool? IsCopy { get; set; }
        public System.DateTime? DueDate { get; set; }
        public Guid? EntityId { get; set; }
        public string EntityType { get; set; }
        public string PONo { get; set; }
        public System.DateTime PostingDate { get; set; }
        public bool? IsGSTApplied { get; set; }
        public bool? IsGSTDeRegistration { get; set; }
        public DateTime? GSTDeRegistrationDate { get; set; }
        public long COAId { get; set; }
        public string ModeOfReceipt { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }
        public decimal BankReceiptAmmount { get; set; }
        public string BankChargesCurrency { get; set; }
        public decimal BankCharges { get; set; }
        public string ExcessPaidByClient { get; set; }
        public string  ExcessPaidByClientCurrency { get; set; }
        public decimal ExcessPaidByClientAmmount { get; set; }
        public string BalancingItemReciveCRCurrency { get; set; }
        public decimal BalancingItemReciveCRAmount { get; set; }
        public string BalancingItemPayDRCurrency { get; set; }
        public decimal BalancingItemPayDRAmount { get; set; }
        public string ReceiptApplicationCurrency { get; set; }
		public string Nature { get; set; }
		public long? CreditTermsId { get; set; }
        public decimal ReceiptApplicationAmmount { get; set; }
        public decimal? BalanceAmount { get; set; }
		public virtual List<JVVDetailModel> JVVDetailModels { get; set; }
        public bool? IsBalancing { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsWithdrawal { get; set; }
        public bool? IsShow { get; set; }
        public string TransferRefNo { get; set; }
        public Nullable<DateTime> ClearingDate { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
        public bool? IsFirst { get; set; }
        public string ActualSysRefNo { get; set; }
    }
}
