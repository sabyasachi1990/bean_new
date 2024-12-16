using AppsWorld.CommonModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.BankTransferModule.Entities
{
    public partial class BankTransferDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BankTransferId { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public int? RecOrder{ get; set; }
        public string ClearingState { get; set; }

        //public virtual BankTransfer BankTransfer { get; set; }

        //public virtual ChartOfAccount ChartOfAccount { get; set; }
    }
}
