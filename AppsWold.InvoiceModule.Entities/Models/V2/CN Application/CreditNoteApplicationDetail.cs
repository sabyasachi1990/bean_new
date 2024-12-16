using Repository.Pattern.Ef6;

namespace AppsWorld.InvoiceModule.Entities.V2
{
    public partial class CreditNoteApplicationDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditNoteApplicationId { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocCurrency { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal? BaseCurrencyExchangeRate { get; set; }
        public string DocDescription { get; set; }
        public long? TaxId { get; set; }
        public long? COAId { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TaxIdCode { get; set; }
        public double? TaxRate { get; set; }
        public int? RecOrder { get; set; }
        public long? ServiceEntityId { get; set; }
        public string DocNo { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}
