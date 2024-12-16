using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using AppsWorld.CommonModule.Entities;

namespace AppsWorld.BankWithdrawalModule.Entities
{
    public partial class WithdrawalDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid WithdrawalId { get; set; }
        public long COAId { get; set; }
        public Nullable<bool> AllowDisAllow { get; set; }
        public Nullable<long> TaxId { get; set; }
        public Nullable<double> TaxRate { get; set; }
        public decimal DocAmount { get; set; }
        public Nullable<decimal> DocTaxAmount { get; set; }
        public decimal DocTotalAmount { get; set; }
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> BaseTaxAmount { get; set; }
        public Nullable<decimal> BaseTotalAmount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public int? RecOrder { get; set; }
        public bool? IsPLAccount { get; set; }
        public string TaxIdCode { get; set; }
        public string ClearingState { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual TaxCode TaxCode { get; set; }
        //public virtual Withdrawal Withdrawal { get; set; }
    }
}
