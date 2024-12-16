using AppsWorld.Framework;
using Newtonsoft.Json;using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BillModule.Entities
{
    public partial class DebitNote : Entity
    {
        public DebitNote()
        {
            this.DebitNoteDetails = new List<DebitNoteDetail>();
            //this.DebitNoteGSTDetails = new List<DebitNoteGSTDetail>();
            this.DebitNoteNotes = new List<DebitNoteNote>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public System.DateTime DocDate { get; set; }
        public long CreditTermsId { get; set; }
        public System.DateTime DueDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public decimal GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public bool IsNoSupportingDocument { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsSegmentReporting { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<short> Version { get; set; }

        public decimal? AllocatedAmount { get; set; }
		//public bool IsGSTDeRegistration { get; set; }

		//public Nullable<System.DateTime> GSTDeRegistrationDate { get; set; }

        [NotMapped]
        public string EntityName { get; set; }

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
        public long? ServiceCompanyId { get; set; }
        public string DebitNoteNumber { get; set; }
        public string DocSubType { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public long? SegmentMasterid1 { get; set; }
        public long? SegmentMasterid2 { get; set; }
        public long? SegmentDetailid1 { get; set; }
        public long? SegmentDetailid2 { get; set; }

        public bool? IsBaseCurrencyRateChanged { get; set; }

        public bool? IsGSTCurrencyRateChanged { get; set; }
		public bool? IsGSTApplied { get; set; }

        public virtual Company Company { get; set; }
        [ForeignKey("CreditTermsId")]
        public virtual TermsOfPayment TermsOfPayment { get; set; }
        public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
        //public virtual ICollection<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }
        public virtual ICollection<DebitNoteNote> DebitNoteNotes { get; set; }
    }
}
