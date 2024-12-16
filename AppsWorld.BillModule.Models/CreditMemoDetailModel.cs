
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class CreditMemoDetailModel
    {
        
        public System.Guid Id { get; set; }
        public System.Guid CreditMemoId { get; set; }
        //public double? Qty { get; set; }
        //public string Unit { get; set; }
        //public decimal? UnitPrice { get; set; }
        //public string DiscountType { get; set; }
        //public Nullable<double> Discount { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        //public string TaxCurrency { get; set; }
        public decimal DocAmount { get; set; }
        //public string AmtCurrency { get; set; }
        public decimal DocTotalAmount { get; set; }
        //public string Remarks { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        [NotMapped]
        public string RecordStatus;
        [NotMapped]
        public string AccountName;
        [NotMapped]
        public string TaxIdCode;
        public string TaxCode { get; set; }
		public string TaxType { get; set; }
        public int? RecOrder { get; set; }
        public string Description { get; set; }
        public bool? IsPLAccount { get; set; }

        //[ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        ////public virtual Invoice Invoice { get; set; }
        //[ForeignKey("TaxId")]
        //public virtual TaxCode TaxCodes { get; set; }
        //public virtual ICollection<InvoiceReceiptModel> InvoiceReceiptModels { get; set; }
    }

}
