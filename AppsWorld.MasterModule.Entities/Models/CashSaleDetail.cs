using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace AppsWorld.MasterModule.Entities
{
    public class CashSaleDetail :Entity
    {
        public CashSaleDetail()
        {
           // this.Item = new Item();
            // this.TaxCode = new TaxCode();
            //this.ChartOfAccount = new ChartOfAccount();
        }
        public System.Guid Id { get; set; }
        public System.Guid CashSaleId { get; set; }
        public Nullable<System.Guid> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
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
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        //public virtual CashSale CashSale { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public bool? IsPLAccount { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public System.Guid ItemId { get; set; }
        // public virtual TaxCode TaxCode { get; set; }
    }
}
