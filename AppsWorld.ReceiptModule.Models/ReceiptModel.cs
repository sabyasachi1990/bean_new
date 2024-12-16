using AppsWorld.Framework;
using AppsWorld.ReceiptModule.Entities;
using FrameWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public class ReceiptModel
    {
        public long ServiceCompanyId { get; set; }
        public Guid? CreditNoteId { get; set; }
        public string CreditNoteDocNumber { get; set; }        
        public string EntityName { get; set; }
        public Nullable<Guid> EntityId { get; set; }
        public long COAId { get; set; }
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string SystemRefNo { get; set; }
        public DateTime DocDate { get; set; }
        public Nullable<DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public bool? NoSupportingDocument { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public string ReceiptRefNo { get; set; }
        public string BankReceiptAmmountCurrency { get; set; }
        public decimal BankReceiptAmmount { get; set; }
        public string BankChargesCurrency { get; set; }
        public decimal? BankCharges { get; set; }
        public string ExcessPaidByClient { get; set; }
        public decimal? CustCreditlimit { get; set; }
        public decimal? ExcessPaidByClientAmmount { get; set; }
        public bool ISMultiCurrency { get; set; }
        public bool IsGstSettings { get; set; }
       // public bool? ISGstDeRegistered { get; set; }
        public string BaseCurrency { get; set; }
        public string DocCurrency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string ModeOfReceipt { get; set; }
        public Nullable<DateTime> ExDurationFrom { get; set; }
        public Nullable<DateTime> GstdurationFrom { get; set; }
        public decimal? GstExchangeRate { get; set; }
        public string GstReportingCurrency { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Version { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public string DocumentState { get; set; }
        public bool? IsEditableExcessPaid { get; set; }
        public string SaveType { get; set; }
        public Nullable<DateTime> FinancialPeriodLockStartDate { get; set; }
        public Nullable<DateTime> FinancialPeriodLockEndDate { get; set; }
        public string PeriodLockPassword { get; set; }
        public string BalancingItemReciveCRCurrency { get; set; }
        public Nullable<decimal> BalancingItemReciveCRAmount { get; set; }
        public string BalancingItemPayDRCurrency { get; set; }
        public Nullable<decimal> BalancingItemPayDRAmount { get; set; }
        public string ExcessPaidByClientCurrency { get; set; }
        public string ExCurrency { get; set; }
        public decimal? GSTTotalAmount { get; set; }
        public string ExtensionType { get; set; }
        public string ReceiptApplicationCurrency { get; set; }
        public Nullable<decimal> ReceiptApplicationAmmount { get; set; }
        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        public string VarianceExchangeRate { get; set; }
        public bool IsVendor { get; set; }
        public bool IsModify { get; set; }
        RecordStatusEnum _status;
        public string DocSubType { get; set; }
        public bool? IsExchangeRateLabel { get; set; }
        public bool? IsDocNoEditable { get; set; }
        public bool? IsInterCompanyActive { get; set; }
        public bool? IsLocked { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        [StatusValue]
        public RecordStatusEnum Status
        {
            get
            {
                return _status;
            }
            set { _status = value; }
        }
        public virtual ICollection<ReceiptBalancingItem> ReceiptBalancingItems { get; set; }
        public List<ReceiptDetailModel> ReceiptDetailModels { get; set; }
    }
}
