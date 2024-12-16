using AppsWorld.CommonModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.CreditMemoModule.Entities
{
    public partial class CreditMemoDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditMemoId { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public string TaxIdCode { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public string TaxCurrency { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        public string  Description { get; set; }
        public Nullable<int> RecOrder { get; set; }
        [ForeignKey("COAId")]
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        public bool? IsPLAccount { get; set; }
        //public virtual CreditMemo CreditMemo { get; set; }
        [ForeignKey("TaxId")]
        public virtual TaxCode TaxCode { get; set; }
        public string ClearingState { get; set; }
    }
}
