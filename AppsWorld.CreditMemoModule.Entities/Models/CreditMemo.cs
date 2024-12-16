using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class CreditMemo:Entity
    {
        public CreditMemo()
        {
            this.CreditMemoApplications = new List<CreditMemoApplication>();
            this.CreditMemoDetails = new List<CreditMemoDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        public DateTime PostingDate { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public Nullable<long> CreditTermsId { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsSegmentReporting { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
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
        public Nullable<System.Guid> ParentInvoiceID { get; set; }
        public string CreditMemoNumber { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public Nullable<bool> IsAllowableDisallowableActivated { get; set; }
        public Nullable<System.DateTime> ReverseDate { get; set; }
        public Nullable<bool> ReverseIsSupportingDocument { get; set; }
        public string ReverseRemarks { get; set; }
        public Nullable<decimal> AllocatedAmount { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTApplied { get; set; }
        public string ExtensionType { get; set; }
        public string DocDescription { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        [ForeignKey("EntityId")]
        public virtual BeanEntity BeanEntity { get; set; }
        public virtual ICollection<CreditMemoApplication> CreditMemoApplications { get; set; }
        public virtual ICollection<CreditMemoDetail> CreditMemoDetails { get; set; }
        public decimal? RoundingAmount { get; set; }
        [NotMapped]
        public string Path { get; set; }

        public int? ClearCount { get; set; }
        public bool? IsLocked { get; set; }
    }
}
