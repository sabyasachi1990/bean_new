using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class CreditMemoApplicationDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditMemoApplicationId { get; set; }
        public System.Guid? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocCurrency { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal? BaseCurrencyExchangeRate { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }

        public string DocDescription { get; set; }
        public long? TaxId { get; set; }
        public long? COAId { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TaxIdCode { get; set; }
        public double? TaxRate { get; set; }
        public int? RecOrder { get; set; }
        public string DocNo { get; set; }
        public decimal? RoundingAmount { get; set; }
        //  public virtual CreditMemoApplication CreditMemoApplication { get; set; }
    }
}
