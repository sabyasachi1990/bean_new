using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FrameWork;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class Receipt : Entity
    {
        public Receipt()
        {
            this.ReceiptBalancingItems = new List<ReceiptBalancingItem>();
            this.ReceiptDetails = new List<ReceiptDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string SystemRefNo { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public System.DateTime DocDate { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public long COAId { get; set; }
        public string ModeOfReceipt { get; set; }
        public string ReceiptRefNo { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }
        public decimal BankReceiptAmmount { get; set; }
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
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsAllowableDisallowable { get; set; }
        public bool IsDisAllow { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public bool? IsVendor { get; set; }
        public string DocCurrency { get; set; }

        public string BaseCurrency { get; set; }

        public bool? IsGSTCurrencyRateChanged { get; set; }
        [NotMapped]
        public string PeriodLockPassword { get; set; }
        public string DocumentState { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }

        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }

        public decimal? VarianceExchangeRate { get; set; }
        public bool? IsExchangeRateLabel { get; set; }
        public int? ClearCount { get; set; }

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
        public bool? IsInterCompanyActive { get; set; }
        public bool? IsLocked { get; set; }
        public virtual ICollection<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; }

    }
}
