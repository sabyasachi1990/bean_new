using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;


namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class JournalEntry : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string ServiceCompany { get; set; }
        public Nullable<long> ServiceId { get; set; }
        public string Nature { get; set; }
        public string DocNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string PONo { get; set; }
        public Nullable<int> CreditTerms { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string EntityType { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string EntityRefNo { get; set; }
        public string EntityName { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<double> Qty { get; set; }
        public Nullable<System.Guid> ItemId { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string DiscountType { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> COAId { get; set; }
        public string TaxCode { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public string TaxType { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> DebitDC { get; set; }
        public Nullable<decimal> CreditDC { get; set; }
        public Nullable<decimal> TaxableamountDC { get; set; }
        public Nullable<decimal> TaxAmountDC { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> ExchangeRateBc { get; set; }
        public Nullable<decimal> DebitBC { get; set; }
        public Nullable<decimal> CreditBC { get; set; }
        public Nullable<decimal> TaxableamountBC { get; set; }
        public Nullable<decimal> TaxAmountBC { get; set; }
        public string GSTReportingCurrency { get; set; }
        public Nullable<decimal> ExchangeRateGSTR { get; set; }
        public Nullable<decimal> DebitGSTR { get; set; }
        public Nullable<decimal> CreditGSTR { get; set; }
        public Nullable<decimal> TaxableamountGSTR { get; set; }
        public Nullable<decimal> TaxAmountGSTR { get; set; }
        public Nullable<bool> Subledgerrequired { get; set; }
        public string SettlementMode { get; set; }
        public Nullable<int> SettlementRefNO { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public string OffsetDocument { get; set; }
        public Nullable<bool> Cleared { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<bool> BankRecon { get; set; }
        public string Reversed { get; set; }
        public string ReversalDocRef { get; set; }
        public Nullable<System.DateTime> ReversalDate { get; set; }
        public string DocumentState { get; set; }
        public Nullable<System.DateTime> DocumentStateDT { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<short> Version { get; set; }
        public long ? ServiceCompanyId { get; set; }
        public Nullable<int> RecOrder { get; set; }

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
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual BeanEntity Entity { get; set; }
        public virtual Item Item { get; set; }
        public virtual Company Company { get; set; }

        //public virtual AppsWorld.CommonModule.Entities.Service Service { get; set; }
    }
}
