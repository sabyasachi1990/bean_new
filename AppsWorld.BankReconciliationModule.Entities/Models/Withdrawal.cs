using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class Withdrawal:Entity
    {
        public Withdrawal()
        {
           // this.WithDrawalDetails = new List<WithDrawalDetail>();
        }

        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string SystemRefNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string DocNo { get; set; }
        public string EntityType { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public long COAId { get; set; }
        public string DocCurrency { get; set; }
        public long ServiceCompanyId { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public Nullable<bool> IsNoSupportingDocumentActivated { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string ModeOfWithDrawal { get; set; }
        public string WithDrawalRefNo { get; set; }
        public decimal BankWithDrawalAmmount { get; set; }
        public Nullable<decimal> BankCharges { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string ExCurrency { get; set; }
        public Nullable<System.DateTime> ExDurationFrom { get; set; }
        public Nullable<System.DateTime> ExDurationTo { get; set; }
        public bool IsGstSettingsActivated { get; set; }
        public bool IsMultiCurrencyActivated { get; set; }
        public bool IsAllowableNonAllowableActivated { get; set; }
        public bool IsSegmentReportingActivated { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<System.DateTime> GSTExDurationFrom { get; set; }
        public Nullable<System.DateTime> GSTExDurationTo { get; set; }
        public Nullable<decimal> GSTTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string DocumentState { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDisAllow { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public string DocDescription { get; set; }
        public Nullable<bool> IsBaseCurrencyRateChanged { get; set; }
        public Nullable<bool> IsGSTCurrencyRateChanged { get; set; }
       // public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual Company Company { get; set; }
      //  public virtual ICollection<WithdrawalDetail> WithdrawalDetails { get; set; }
    }
}
