using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DoubtfulDebtModel
    {
        public DoubtfulDebtModel()
        {
            this.DoubtfulDebtAllocations = new List<DoubtfulDebtAllocationModel>();
        }
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }     
        public string DocSubType { get; set; }
        public string DocumentState { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        //public string EvenStatus { get; set; } 
        public Guid EntityId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string Nature { get; set; }
        //Field applicable only if "Multi-Currency" module is activated
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public decimal BalanceAmount { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; } 
        public bool IsMultiCurrency { get; set; }
        public string Version { get; set; }
        //public bool? IsSegmentReporting { get; set; }
        //public bool? IsAllowableNonAllowable { get; set; }
        //public bool? IsAllowableDisallowableActivated { get; set; }
        public string EntityType { get; set; }
        public decimal GrandTotal { get; set; }
        public string PeriodLockPassword { get; set; }
        public string InvoiceNumber { get; set; }
        public string EntityName { get; set; }

        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string Remarks { get; set; }
        //public long? SegmentMasterid1 { get; set; }
        //public long? SegmentMasterid2 { get; set; }
        //public long? SegmentDetailid1 { get; set; }
        //public long? SegmentDetailid2 { get; set; }
        //public bool? IsSegmentActive1 { get; set; }
        //public bool? IsSegmentActive2 { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
		public bool? IsGSTApplied { get; set; }
		//public decimal? ItemTotal { get; set; }
		public string SaveType { get; set; }
		public string ExtensionType { get; set; }
		//public DateTime? DeRegistrationDate { get; set; }
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
        public Guid JournalId { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public bool? IsLocked { get; set; }
        //public string JounalSystemreferenceNo { get; set; }
        //[ForeignKey("EntityId")]
        //public virtual BeanEntity BeanEntity { get; set; }
        //[ForeignKey("CreditTermsId")]
        //public virtual TermsOfPayment TermsOfPayments { get; set; }
        public virtual DoubtfulDebtAllocationModel DoubtfulDebtAllocation { get; set; }
        public virtual ICollection<DoubtfulDebtAllocationModel> DoubtfulDebtAllocations { get; set; }
    }
}
