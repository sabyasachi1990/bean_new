using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.CommonModule.Models;

namespace AppsWorld.PaymentModule.Models
{
    public class PaymentModel
    {
        //public PaymentModel()
        //{
        //    this.COAModels = new COAModel();
        //    this.CurrencyModels = new CurrencyModel();
        //    this.EntityModels = new EntityModel();
        //    this.ModeOfPaymentModels = new ModeOfPaymentModel();
        //    this.ServiceCompanyMOdels = new ServiceCompanyModel();
        //}
        //public COAModel COAModels { get; set; }
        //public CurrencyModel CurrencyModels { get; set; }
        //public CreditTermModel CreditTermsModels { get; set; }
        //public EntityModel EntityModels { get; set; }
        //public ModeOfPaymentModel ModeOfPaymentModels { get; set; }
        //public ServiceCompanyModel ServiceCompanyMOdels { get; set; }
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string SystemRefNo { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public string PaymentRefNo { get; set; }
        public string BankPaymentAmmountCurrency { get; set; }
        public decimal? BankPaymentAmmount { get; set; }
        public string BankChargesCurrency { get; set; }
        //public decimal? BankCharges { get; set; }
        //public bool IsAllowDisAllow { get; set; }
        //public bool IsDisAllow { get; set; }
        //public string ExcessPaidByClient { get; set; }
        //public decimal? ExcessPaidByClientAmmount { get; set; }
        public bool ISMultiCurrency { get; set; }
        public bool IsGstSettings { get; set; }
        //public bool? ISGstDeRegistered { get; set; }
        public string BaseCurrency { get; set; }
        public string DocCurrency { get; set; }
        [DataType("decimal(15,10")]
        public decimal? ExchangeRate { get; set; }
        public Nullable<DateTime> ExDurationFrom { get; set; }
        public Nullable<DateTime> ExDurationTo { get; set; }
        public Nullable<DateTime> GstdurationFrom { get; set; }
        public Nullable<DateTime> GstDurationTo { get; set; }
        public string GstReportingCurrency { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        //public bool IsAllowableNonAllowable { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public string DocumentState { get; set; }
        public string PeriodLockPassword { get; set; }
        //public Nullable<DateTime> FinancialStartDate { get; set; }
        //public Nullable<DateTime> FinancialEndDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        //public string BalancingItemReciveCRCurrency { get; set; }
        //public Nullable<decimal> BalancingItemReciveCRAmount { get; set; }
        //public string BalancingItemPayDRCurrency { get; set; }
        //public Nullable<decimal> BalancingItemPayDRAmount { get; set; }
        //public string ExcessPaidByClientCurrency { get; set; }
        public string ExCurrency { get; set; }
        //public decimal? GSTTotalAmount { get; set; }
        public string PaymentApplicationCurrency { get; set; }
        public Nullable<decimal> PaymentApplicationAmmount { get; set; }
        [DataType("decimal(15,10)")]
        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        public string VarianceExchangeRate { get; set; }
        //public bool? IsGSTApplied { get; set; }
        //public DateTime? DeRegistrationDate { get; set; }
        public bool IsBankReconciliation { get; set; }
        public string ModeOfPayment { get; set; }
        public string DocSubType { get; set; }
        //public System.Guid JournalId { get; set; }
        public bool? IsExchangeRateLabel { get; set; }
        public bool? IsDocNoEditable { get; set; }
        //public string EntityType { get; set; }
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        //public string COAName { get; set; }
        //public string ServiceCompanyName { get; set; }
        public long ServiceCompanyId { get; set; }
        public bool? IsInterCompanyActive { get; set; }
        //public string code { get; set; }
        public long COAId { get; set; }
        public string DocType { get; set; }
        public string ExtensionType { get; set; }
        public bool IsModify { get; set; }

        public bool? IsCustomer { get; set; }
        public bool? IsLocked { get; set; }
        //public string JournalRefNo { get; set; }
        //public string JounalSystemreferenceNo { get; set; }
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
        public List<TailsModel> TileAttachments { get; set; }
        public string Path { get; set; }
        public List<PaymentDetailModel> PaymentDetailModels { get; set; }
        //public List<JVViewModel> JVViewModels { get; set; }
    }
}
