using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class InvoiceDetail
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public System.Guid? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string DiscountType { get; set; }
        public Nullable<double> Discount { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public string TaxCurrency { get; set; }
        public decimal DocAmount { get; set; }
        public string AmtCurrency { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Remarks { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public int? RecOrder { get; set; }
        public bool? IsPLAccount { get; set; }
        public string TaxIdCode { get; set; }
        [NotMapped]
        public string RecordStatus;

    }
}
