using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class DoubtfulDebtAllocationDetail:Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid DoubtfulDebtAllocationId { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocCurrency { get; set; }
        public decimal AllocateAmount { get; set; }
        public virtual DoubtfulDebtAllocation DoubtfulDebtAllocation { get; set; }
    }
}