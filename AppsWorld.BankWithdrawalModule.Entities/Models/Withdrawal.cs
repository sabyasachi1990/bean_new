using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using AppsWorld.CommonModule.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AppsWorld.Framework;
using FrameWork;

namespace AppsWorld.BankWithdrawalModule.Entities
{
    public partial class Withdrawal : Entity
    {
        public Withdrawal()
        {
            this.WithdrawalDetails = new List<WithdrawalDetail>();
           // this.GSTDetails = new List<GSTDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string SystemRefNo { get; set; }
        public string EntityType { get; set; }
        public System.Guid? EntityId { get; set; }
        public long COAId { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string DocCurrency { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<bool> IsNoSupportingDocumentActivated { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string ModeOfWithDrawal { get; set; }
        public string WithDrawalRefNo { get; set; }
        public decimal BankWithdrawalAmmount { get; set; }
        public Nullable<decimal> BankCharges { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public bool IsGstSettingsActivated { get; set; }
        public bool IsMultiCurrencyActivated { get; set; }
        public bool IsAllowableNonAllowableActivated { get; set; }
        public bool IsSegmentReportingActivated { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocumentState { get; set; }
        public string Remarks { get; set; }
        public bool? IsLocked { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> IsDisAllow { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public int? RecOrder { get; set; }
        public string  DocDescription { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
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
        //[ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual Entity Entity { get; set; }
        //public virtual Company Company { get; set; }
        public virtual List<WithdrawalDetail> WithdrawalDetails { get; set; }
        //public virtual List<GSTDetail> GSTDetails { get; set; }
    }
}
