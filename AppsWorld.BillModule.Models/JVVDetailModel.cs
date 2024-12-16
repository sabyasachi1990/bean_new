using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class JVVDetailModel
    {
        public System.Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid DocumentDetailId { get; set; }
		public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
		public string DocType { get; set; }
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
		public string SystemReferenceNo { get; set; }
        public string SystemRefNo { get; set; }
        public decimal DocCreditTotal { get; set; }
		public string BaseCurrency { get; set; }
		public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> BaseDebitTotal { get; set; }
        public Nullable<decimal> BaseCreditTotal { get; set; }
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double Qty { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public string DiscountType { get; set; }
        public double Discount { get; set; }
        public string AccountName { get; set; }
		public string TaxCode { get; set; }
		public string GSTExCurrency { get; set; }
		public Nullable<decimal> GSTExchangeRate { get; set; }
		public Nullable<System.DateTime> ExDurationFrom { get; set; }
		public Nullable<System.DateTime> ExDurationTo { get; set; }
		public Nullable<decimal> DocTaxableAmount { get; set; }
		public Nullable<decimal> DocTaxAmount { get; set; }
		public Nullable<decimal> BaseTaxableAmount { get; set; }
		public Nullable<decimal> BaseTaxAmount { get; set; }
		public string DocSubType { get; set; }
		public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public long ServiceCompanyId { get; set; }
		public string Nature { get; set; }
		public string OffsetDocument { get; set; }
		public bool IsTax { get; set; }
        public DateTime? PostingDate { get; set; }
        public string DocCurrency { get; set; }
		public string AccountCode { get; set; }
		public Nullable<decimal> GSTDebit { get; set; }
		public Nullable<decimal> GSTCredit { get; set; }
		public Nullable<decimal> GSTTaxableAmount { get; set; }
		public Nullable<decimal> GSTTaxAmount { get; set; }
        public long? SegmentMasterid1 { get; set; }
        public long? SegmentMasterid2 { get; set; }
        public long? SegmentDetailid1 { get; set; }
        public long? SegmentDetailid2 { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public int? RecOrder { get; set; }
        public decimal? AmountDue { get; set; }
        public string DocDescription { get; set; }
        public long? CreditTermsId { get; set; }
        public decimal? BaseAmount { get; set; }
    }
}
