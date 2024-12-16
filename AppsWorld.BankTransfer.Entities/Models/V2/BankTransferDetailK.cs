using Repository.Pattern.Ef6;
using System;

namespace AppsWorld.BankTransferModule.Entities.V2
{
    public partial class BankTransferDetailK : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BankTransferId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
         
    }
}
