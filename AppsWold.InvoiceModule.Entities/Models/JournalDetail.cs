using System;
using Repository.Pattern.Ef6;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities
{
    public partial class JournalDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        //public long COAId { get; set; }
        //public string AccountDescription { get; set; }
        //public decimal? DocDebit { get; set; }
        //public decimal? DocCredit { get; set; }
        //public decimal? DocTaxDebit { get; set; }
        //public decimal? DocTaxCredit { get; set; }
        //public Nullable<decimal> BaseDebit { get; set; }
        //public Nullable<decimal> BaseCredit { get; set; }
        //public Nullable<decimal> BaseTaxDebit { get; set; }
        //public Nullable<decimal> BaseTaxCredit { get; set; }
        //public decimal DocDebitTotal { get; set; }
        //public decimal DocCreditTotal { get; set; }
        //public Nullable<decimal> BaseDebitTotal { get; set; }
        //public Nullable<decimal> BaseCreditTotal { get; set; }
		public Guid? DocumentId { get; set; }
        //public string DocType { get; set; }
        //public string DocSubType { get; set; }
        //public string DocNo { get; set; }
        //public long? ServiceCompanyId { get; set; }
        public Guid? DocumentDetailId { get; set; }
        //public string  SystemRefNo { get; set; }
        //public DateTime? DocDate { get; set; }
        //public string DocCurrency { get; set; }
        //public string BaseCurrency { get; set; }
        //public string Type { get; set; }
        public string ClearingStatus { get; set; }
        public int? RecOrder { get; set; }
        //public bool? IsTax { get; set; }
        public long? TaxId { get; set; }
        public decimal? AmountDue { get; set; }
        public Nullable<DateTime> ClearingDate { get; set; }
    }
}
