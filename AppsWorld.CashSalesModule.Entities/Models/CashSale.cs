using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CommonModule.Entities;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.CashSalesModule.Entities.Models
{
    public partial class CashSale :Entity
    {
        public CashSale()
        {
            this.CashSaleDetails = new List<CashSaleDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
       
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public string EntityType { get; set; }
        public System.Guid? EntityId { get; set; }
        public string ModeOfReceipt { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string ReceiptrefNo { get; set; }
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
        //public Nullable<int> Status { get; set; }
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
       
        public string CashSaleNumber { get; set; }
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
        public Nullable<decimal> ItemTotal { get; set; }
        public string ExtensionType { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string DocDescription { get; set; }
        public long COAId { get; set; }
        public bool? IsLocked { get; set; }
        //[ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual ICollection<CashSaleDetail> CashSaleDetails { get; set; }
    }
}
