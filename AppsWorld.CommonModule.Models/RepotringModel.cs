using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class RepotringModel
    {
        public DateTime? DocDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountClass { get; set; }
        public string AccountCategory { get; set; }
        public string AccountSubCategory { get; set; }
        public string AccountNature { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string PONo { get; set; }
        public string DocNo { get; set; }
        public string SystemRefNo { get; set; }
        public string Nature { get; set; }
        public string DocDescription { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string CorrAccountName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public string DiscountType { get; set; }
        public Nullable<double> Discount { get; set; }
        public string DocCurrency { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DocBalance { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public decimal? BaseBalance { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public long? CreditTermsDays { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public Nullable<DateTime> BankClearing { get; set; }
        public string ModeOfReceipt { get; set; }
        public string BankReconcile { get; set; }
        public string ReversalDocRefNo { get; set; }
        public DateTime? ReversalDate { get; set; }
        public string ClearingStatus { get; set; }
        public string  CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        //public string AccountDescription { get; set; }
        ////public Nullable<bool> AllowDisAllow { get; set; }
        //public string AllowDisAllow { get; set; }
        
        //public string TaxCode { get; set; }
        //public decimal? BaseAmount { get; set; }
        //public Nullable<System.DateTime> PostingDate { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        //public int? RecOrder { get; set; }
        //public string ServiceEntityName { get; set; }
        //public bool? IsMultiCurrencyActive { get; set; }
        //public bool? IsSegmentActive { get; set; }
        //public string VendorType { get; set; }
        //public bool? IsNoSupprotingDocs { get; set; }
        //public string NoSupprotingDocs { get; set; }
        //public string COA_Parameter { get; set; }
        //public bool? IsSubTotal { get; set; }
        //public int? IsBF { get; set; }
        //public int? ClasssOrder { get; set; }
       

    }
}
