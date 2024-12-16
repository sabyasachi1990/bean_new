using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class WithdrawalModel
    {
        public WithdrawalModel()
        {
            //this.COAModels = new COAModel();
            ////this.CurrencyModels = new CurrencyModel();
            ////this.CreditTermsModels = new CreditTermModel();
            //this.EntityModels = new EntityModel();
            //this.ModeOfWithdrawalModels = new ModeOfWithdrawalModel();
            //this.ServiceCompanyModels = new ServiceCompanyModel();
        }
        //public COAModel COAModels { get; set; }
        ////public CurrencyModel CurrencyModels { get; set; }
        //public CreditTermModel CreditTermsModels { get; set; }
        //public EntityModel EntityModels { get; set; }
        //public ModeOfWithdrawalModel ModeOfWithdrawalModels { get; set; }
        //public ServiceCompanyModel ServiceCompanyModels { get; set; }

        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string SystemRefNo { get; set; }
        public DateTime DocDate { get; set; }
        public string DocType { get; set; }
        public Nullable<DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public string DocDescription { get; set; }       
        public bool? NoSupportingDocument { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public string WithdrawalRefNo { get; set; }
        //public decimal WithdrawalAmmount { get; set; }
        public bool IsAllowableNonAllowableActivated { get; set; }
        public bool? IsDisAllow { get; set; }
        //public string ExcessPaidByClient { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public bool IsGstSettingsActivated { get; set; }
        public bool IsMultiCurrencyActivated { get; set; }
        public bool? ISGstDeRegistered { get; set; }
        public string BaseCurrency { get; set; }
        public string DocCurrency { get; set; }
        [DataType("decimal(15,10")]
        public decimal? ExchangeRate { get; set; }
        public Nullable<DateTime> ExDurationFrom { get; set; }
        public Nullable<DateTime> ExDurationTo { get; set; }
        [DataType("decimal(15,10")]
        public decimal? GstExchangeRate { get; set; }
        public string GstReportingCurrency { get; set; }
        public decimal GrandTotal { get; set; }
        //public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public string DocumentState { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public decimal BankWithdrawalAmmount { get; set; }
        //public bool IsSegmentReportingActivated { get; set; }
        public decimal? GSTTotalAmount { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public bool? IsGSTApplied { get; set; }
        public long ServiceCompanyId { get; set; }
        public Guid? EntityId { get; set; }
        public string ModeOfWithdrawal { get; set; }
        public string EntityType { get; set; }
        public long COAId { get; set; }
        //public string code { get; set; }
        public string EntityName { get; set; }
        public bool? IsLocked { get; set; }
        //public DateTime? DeRegistrationDate { get; set; }
        //public bool? IsSegmentActive1 { get; set; }
        //public bool? IsSegmentActive2 { get; set; }
        //public Nullable<long> SegmentMasterid1 { get; set; }
        //public Nullable<long> SegmentMasterid2 { get; set; }
        //public Nullable<long> SegmentDetailid1 { get; set; }
        //public Nullable<long> SegmentDetailid2 { get; set; }
        //public bool IsBankReconciliation { get; set; }
        //public bool IsWithdrawalEdit { get; set; }
        public bool? IsDocNoEditable { get; set; }
        RecordStatusEnum _status;
        public Guid JournalId { get; set; }
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
        public List<TailsModel> TileAttachments { get; set; }
        public string Path { get; set; }
        public virtual List<WithdrawalDetailModel> WithdrawalDetailModels { get; set; }
        //public virtual List<GSTDetailModel> GSTDetailModels { get; set; }
        
    }
}
