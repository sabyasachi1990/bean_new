using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.BankTransferModule.Entities.V2
{
    public partial class BankTransferK : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime TransferDate { get; set; }
        public string DocNo { get; set; }
        public string ModeOfTransfer { get; set; }
        public string TransferRefNo { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public string DocumentState { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [ForeignKey("BankTransferId")]
        public virtual List<BankTransferDetailK> BankTransferDetails { get; set; }
    }
}
