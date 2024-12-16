using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Models
{
    public class CashSaleDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid CashSaleId { get; set; }
        public Nullable<System.Guid> ItemId { get; set; }
        //public string ItemCode { get; set; }
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
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        public Nullable<int> RecOrder { get; set; }
        public bool? IsPLAccount { get; set; }
        public virtual List<GSTDetailModel> GstDetailModels { get; set; }
        public string TaxType { get; set; }
        public string AccountName { get; set; }
        public string RecordStatus { get; set; }
        public string TaxIdCode { get; set; }
        public string ClearingState { get; set; }
    }
}
