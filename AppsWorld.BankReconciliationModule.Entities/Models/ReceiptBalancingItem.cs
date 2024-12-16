using System;
using System.Collections.Generic;
using AppsWorld.BankReconciliationModule.Entities;
using System.Data.Entity;
using AppsWorld.BankReconciliationModule.Entities.Models;
using Repository.Pattern.Ef6;
using AppsWorld.BankReconciliationModule.RepositoryPattern;

namespace AppsWorld.BankReconciliationModule.Entities
{
    public partial class ReceiptBalancingItem:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid ReceiptId { get; set; }
        public string ReciveORPay { get; set; }
        public long COAId { get; set; }
        public string Account { get; set; }
        public Nullable<bool> IsDisAllow { get; set; }
        public long TaxId { get; set; }
        public string TaxCode { get; set; }
        public double TaxRate { get; set; }
        public string TaxType { get; set; }
        public string Currency { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public string Remarks { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int Status { get; set; }
       // public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual Receipt Receipt { get; set; }
        public virtual TaxCode TaxCode1 { get; set; }
    }
}
