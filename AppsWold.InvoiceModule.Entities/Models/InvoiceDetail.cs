using AppsWorld.CommonModule.Entities;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.InvoiceModule.Entities
{
    //using Repository.Pattern.Ef6;
    //using System;
    //using System.Collections.Generic;
    //using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    // [Table("Bean.InvoiceDetail")]
    public partial class InvoiceDetail : Entity
    {
        public InvoiceDetail()
        {
            //this.Item = new Item();
           // this.TaxCode = new TaxCode();
            //this.ChartOfAccount = new ChartOfAccount();
        }
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
        //public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        //public string TaxCurrency { get; set; }
        public decimal DocAmount { get; set; }
        public string AmtCurrency { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Remarks { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? BaseTaxAmount { get; set; }
        public decimal? BaseTotalAmount { get; set; }
        public int? RecOrder { get; set; }
        //public bool? IsPLAccount { get; set; }
        public string TaxIdCode { get; set; }
        [NotMapped]
        public string RecordStatus;
        public string ClearingState { get; set; }

        //[NotMapped]
        //public string AccountName;

        //      [NotMapped]
        //      public string TaxIdCode;

        //[NotMapped]
        //public string TaxType;

        //      //[ForeignKey("ItemId")]
        //      //public virtual Item Item { get; set; }
        //[ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        ////public virtual Invoice Invoice { get; set; }
        //[ForeignKey("TaxId")]
        //public virtual TaxCode TaxCodes { get; set; }


    }
}