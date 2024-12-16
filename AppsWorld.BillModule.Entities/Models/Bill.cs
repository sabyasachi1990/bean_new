using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BillModule.Entities
{
    public partial class Bill : Entity
    {
        public Bill()
        {
            this.BillDetails = new List<BillDetail>();
            //this.BillGSTDetails = new List<BillGSTDetail>();
            //this.BillCreditMemos = new BillCreditMemo();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string SystemReferenceNumber { get; set; }
        public System.DateTime PostingDate { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public string DocumentState { get; set; }
        public System.Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public long? CreditTermsId { get; set; }
        public int CreditTermValue { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public decimal GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsNoSupportingDocument { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsSegmentReporting { get; set; }
        public bool IsAllowableDisallowable { get; set; }
        public bool IsGSTCurrencyRateChanged { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public bool? IsGSTApplied { get; set; }
        public string VendorType { get; set; }
        //public bool IsGSTDeRegistration { get; set; }
        public string DocDescription { get; set; }
        public Guid? PayrollId { get; set; }
        //public Nullable<System.DateTime> GSTDeRegistrationDate { get; set; }
        public int? ClearCount { get; set; }
        public string DocType { get; set; }
        //public bool? IsOBBill { get; set; }
        public bool? IsExternal { get; set; }
        public Guid? OpeningBalanceId { get; set; }
        public bool? IsLocked { get; set; }

        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }

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
        public virtual Company Company { get; set; }
        //public virtual TermsOfPayment TermsOfPayment { get; set; }
        public virtual BeanEntity Entity { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        //public virtual ICollection<BillGSTDetail> BillGSTDetails { get; set; }
        //public virtual BillCreditMemo BillCreditMemos { get; set; }
        public Guid? SyncHRPayrollId { get; set; }
        public string SyncHRPayrollStatus { get; set; }
        public DateTime? SyncHRPayrollDate { get; set; }
        public string SyncHRPayrollRemarks { get; set; }
        public decimal? roundingamount { get; set; }


        public System.Guid? PeppolDocumentId { get; set; }  //Peppol Related

    }
}
