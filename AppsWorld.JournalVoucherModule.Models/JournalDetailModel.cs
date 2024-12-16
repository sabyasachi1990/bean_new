using AppsWorld.JournalVoucherModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations.Schema;



namespace AppsWorld.JournalVoucherModule.Model
{
    public partial class JournalDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid JournalId { get; set; }
        public long COAId { get; set; }
        public string AccountDescription { get; set; }


        public Nullable<bool> AllowDisAllow { get; set; }
        public string TaxName { get; set; }
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
        public string RecordStatus { get; set; }
        public string TaxCode { get; set; }
        public string RecieptType { get; set; }
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual TaxCode TaxCodes { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public decimal? DocumentAmount { get; set; }
        public bool? IsPLAccount { get; set; }
        public Nullable<Guid> EntityId { get; set; }
        public string EntityName { get; set; }
        //[NotMapped]
        public string TaxIdCode { get; set; }
        //public string ClearingState { get; set; }
        public string ClearingStatus { get; set; }
        public decimal? GSTTaxCredit { get; set; }
        public decimal? GSTTaxDebit { get; set; }
    }
}
