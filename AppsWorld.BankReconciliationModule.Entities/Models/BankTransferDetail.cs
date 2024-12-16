using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Entities.Models
{
    public class BankTransferDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid BankTransferId { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public int? RecOrder { get; set; }
    }
}
