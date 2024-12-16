using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
   public  class InvoiceModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string DocumentState { get; set; }
        public System.DateTime? DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        public Guid? EntityId { get; set; }
        public long? CreditTermsId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string Nature { get; set; }
        //Field applicable only if "Multi-Currency" module is activated
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public decimal BalanceAmount { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public bool IsRepeatingInvoice { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsSegmentReporting { get; set; }
        public bool IsAllowableNonAllowable { get; set; }

        public string EntityType { get; set; }
        public Nullable<int> RepEveryPeriodNo { get; set; }
        public string RepEveryPeriod { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public decimal? GSTTotalAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public string PeriodLockPassword { get; set; }

        public Nullable<System.Guid> ParentInvoiceID { get; set; }

        public string InvoiceNumber { get; set; }
        public string EntityName { get; set; }
        public bool? IsDeleted { get; set; }
        
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }

        public string Remarks { get; set; }

        //public bool IsGSTDeregistered { get; set; }

        public string DocType { get; set; }

        //public decimal? AllocatedAmount { get; set; }

        public bool? IsBaseCurrencyRateChanged { get; set; }

        public bool? IsGSTCurrencyRateChanged { get; set; }

        public bool? IsGSTApplied { get; set; }

        public decimal? CustCreditlimit { get; set; }

        //public DateTime? DeRegistrationDate { get; set; }
        public string DocDescription { get; set; }
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
        //public Guid JournalId { get; set; }
        public bool? IsOBInvoice { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public Guid? OpeningBalanceId { get; set; }

        //for Bill
        public DateTime? PostingDate { get; set; }
        public bool ISAllowDisAllow { get; set; }
        public string VendorType { get; set; }
        public decimal? PaymentTotalAmount { get; set; }
        public decimal? MemoTotalAmount { get; set; }
        public System.DateTime DateFrom { get; set; }
        public System.DateTime Dateto { get; set; }
        public bool? IsExternal { get; set; }
        public string GstReportingCurrency { get; set; }


        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }

        public List<BillDetailModel> BillDetailModels { get; set; }
    }
}
