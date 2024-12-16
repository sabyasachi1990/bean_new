using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class Payment:Entity
    {
        public Payment()
        {
        //    this.PaymentDetails = new List<PaymentDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public string SystemRefNo { get; set; }
        public string EntityType { get; set; }
        public System.Guid EntityId { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public System.DateTime DocDate { get; set; }
        public System.DateTime DueDate { get; set; }
        public string DocNo { get; set; }
        public long ServiceCompanyId { get; set; }
        public Nullable<bool> IsNoSupportingDocument { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public long COAId { get; set; }
        public string ModeOfReceipt { get; set; }
        public string PaymentRefNo { get; set; }
        public string BankPaymentAmmountCurrency { get; set; }
        public Nullable<decimal> BankPaymentAmmount { get; set; }
        public string BankChargesCurrency { get; set; }
        public Nullable<decimal> BankCharges { get; set; }
        public string ExcessPaidByClient { get; set; }
        public string ExcessPaidByClientCurrency { get; set; }
        public Nullable<decimal> ExcessPaidByClientAmmount { get; set; }
        public string BalancingItemPaymentCRCurrency { get; set; }
        public Nullable<decimal> BalancingItemPaymentCRAmount { get; set; }
        public string BalancingItemPayDRCurrency { get; set; }
        public Nullable<decimal> BalancingItemPayDRAmount { get; set; }
        public string PaymentApplicationCurrency { get; set; }
        public Nullable<decimal> PaymentApplicationAmmount { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public bool IsGstSettings { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool? IsAllowableDisallowable { get; set; }
        public bool? IsDisAllow { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public string DocumentState { get; set; }
        public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        public Nullable<decimal> VarianceExchangeRate { get; set; }
        public Nullable<bool> IsGSTApplied { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<int> Status { get; set; }
       // public virtual ChartOfAccount ChartOfAccount { get; set; }
       // public virtual Entity Entity { get; set; }
        public virtual Company Company { get; set; }
     //   public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
