using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.Framework;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class DebitNoteModel
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public string Nature { get; set; }
        public System.DateTime DocDate { get; set; }
        public long CreditTermsId { get; set; }
        public System.DateTime DueDate { get; set; }
        public string DocNo { get; set; }
        public string PONo { get; set; }
        //public string EventStatus { get; set; }

        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public string DocumentState { get; set; }
        public decimal GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocument { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsSegmentReporting { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public string EntityName { get; set; }
        public decimal? CustCreditlimit { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public bool IsGSTDeregistered { get; set; }
        public string BaseCurrency { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string DebitNoteNumber { get; set; }

        public decimal? AllocatedAmount { get; set; }
        //public long? SegmentMasterid1 { get; set; }
        //public long? SegmentMasterid2 { get; set; }
        //public long? SegmentDetailid1 { get; set; }
        //public long? SegmentDetailid2 { get; set; }
        //public bool? IsSegmentActive1 { get; set; }
        //public decimal? ReceiptTotalAmount { get; set; }
        //public DateTime? DeRegistrationDate { get; set; }
        //public bool? IsSegmentActive2 { get; set; }

        public bool? IsBaseCurrencyRateChanged { get; set; }

        public bool? IsGSTCurrencyRateChanged { get; set; }

        //public bool? IsProvision { get; set; }
        //public decimal? ProvisionCount { get; set; }
        //public bool? IsCreditNote { get; set; }
        //public bool IsDoubtfulDebtShow { get; set; }
        public bool? IsGSTApplied { get; set; }
        //public decimal? CreditNoteTotalAmount { get; set; }

        //public decimal? DoubtfulDebitTotalAmount { get; set; }
        //public virtual Company Company { get; set; }
        //[ForeignKey("CreditTermsId")]
        //public virtual TermsOfPayment TermsOfPayment { get; set; }
        //[ForeignKey("EntityId")]

        RecordStatusEnum _status;
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [FrameWork.StatusValue]
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
        //public virtual BeanEntity Entity { get; set; }
        public virtual ICollection<DebitNoteDetail> DebitNoteDetails { get; set; }
        public virtual ICollection<DebitNoteNote> DebitNoteNotes { get; set; }
        //public virtual ICollection<DebitNoteGSTDetail> DebitNoteGSTDetails { get; set; }
        //public virtual ICollection<ProvisionModel> ProvisionModels { get; set; }

        //public virtual CreditNoteModel CreditNoteModel { get; set; }
        //public virtual ICollection<CreditNoteDetailModel> CreditNoteDetailModels { get; set; }

        //public virtual ICollection<DebitNoteCreditNoteModel> DebitNoteCreditNoteModels { get; set; }

        //public virtual ICollection<DebitNoteDoubtFulDebtModel> DebitNoteDoubtFulDebtModels { get; set; }

        #region for_lookup_call
        //public CommonModule.Infra.LookUpCategory<string> CurrencyLU { get; set; }
        ////public LookUpCategory<string> NatureLU { get; set; }
        //public List<string> NatureLU { get; set; }
        ////public LookUpCategory<string> AllowableNonAllowableLU { get; set; }
        //public List<CommonModule.Infra.LookUp<string>> TermsOfPaymentLU { get; set; }
        //public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        //public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        //public List<COALookup<string>> ChartOfAccountLU { get; set; }
        #endregion





    }
}
