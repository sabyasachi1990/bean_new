using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Entities.Models
{
    public partial class JournalDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public long COAId { get; set; }
        public string AccountDescription { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DocTaxDebit { get; set; }
        public decimal? DocTaxCredit { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<decimal> BaseTaxDebit { get; set; }
        public Nullable<decimal> BaseTaxCredit { get; set; }
        public decimal DocDebitTotal { get; set; }
        public decimal DocCreditTotal { get; set; }
        public Nullable<decimal> BaseDebitTotal { get; set; }
        public Nullable<decimal> BaseCreditTotal { get; set; }
        public Nullable<System.Guid> DocumentId { get; set; }
        public Nullable<System.Guid> DocumentDetailId { get; set; }
        public Nullable<System.Guid> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string DiscountType { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<decimal> DocumentAmount { get; set; }
        public Nullable<decimal> AmountDue { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> AmountToAllocate { get; set; }
        public Nullable<decimal> DocTaxableAmount { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public Nullable<decimal> BaseTaxableAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string GSTExCurrency { get; set; }
        public Nullable<decimal> GSTExchangeRate { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public string DocNo { get; set; }
        public string Nature { get; set; }
        public string OffsetDocument { get; set; }
        public Nullable<bool> IsTax { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string SettlementMode { get; set; }
        public string SettlementRefNo { get; set; }
        public Nullable<System.DateTime> SettlementDate { get; set; }
        public string SystemRefNo { get; set; }
        public string Remarks { get; set; }
        public string PONo { get; set; }
        public Nullable<long> CreditTermsId { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<long> SegmentMasterid2 { get; set; }
        public Nullable<long> SegmentDetailid2 { get; set; }
        public Nullable<long> SegmentDetailid1 { get; set; }
        public Nullable<long> SegmentMasterid1 { get; set; }
        public Nullable<bool> NoSupportingDocs { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public decimal? BaseAmount { get; set; }
        public string DocCurrency { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocDescription { get; set; }
        public string ClearingStatus { get; set; }
        public DateTime? ClearingDate { get; set; }
        public DateTime? BankClearingDate { get; set; }
        public int? RecOrder { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        //public virtual Journal Journal { get; set; }
    }
}
